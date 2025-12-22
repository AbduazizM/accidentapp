using Microsoft.EntityFrameworkCore;
using SKARB.Web.Data;
using SKARB.Web.Models;

namespace Pregnancy.Tests;

/// <summary>
/// Интеграционные тесты для работы с базой данных через DbContext
/// Тестируемый модуль: SKARB.Web.Data.SkarbDbContext
/// Цель: Проверка корректности операций CRUD с базой данных
/// Используется InMemory база данных для изоляции тестов
/// </summary>
public class DatabaseIntegrationTests
{
    /// <summary>
    /// Создает тестовый контекст базы данных в памяти
    /// Каждый тест получает свою изолированную базу данных
    /// </summary>
    private SkarbDbContext CreateInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<SkarbDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Уникальная БД для каждого теста
            .Options;

        return new SkarbDbContext(options);
    }

    /// <summary>
    /// Тест 1: Проверка добавления персоны в базу данных
    /// Ожидаемый результат: Персона должна быть сохранена и доступна для чтения
    /// Тестовые данные: Новая персона с полными данными
    /// </summary>
    [Fact]
    public async Task Database_AddPerson_ShouldSaveAndRetrieveSuccessfully()
    {
        // Arrange - создаем контекст и персону
        using var context = CreateInMemoryContext();
        var person = new Person
        {
            LastName = "Петров",
            FirstName = "Петр",
            MiddleName = "Петрович",
            Address = "г. Санкт-Петербург, Невский проспект, д. 1",
            Convictoins = 0,
            RegNumber = "REG-TEST-001"
        };

        // Act - добавляем персону в базу
        context.People.Add(person);
        await context.SaveChangesAsync();

        // Assert - проверяем, что персона сохранена
        var savedPerson = await context.People.FirstOrDefaultAsync(p => p.RegNumber == "REG-TEST-001");
        Assert.NotNull(savedPerson);
        Assert.Equal("Петров", savedPerson.LastName);
        Assert.Equal("Петр", savedPerson.FirstName);
        Assert.True(savedPerson.PersonId > 0); // ID должен быть присвоен автоматически
    }

    /// <summary>
    /// Тест 2: Проверка добавления инцидента с решением
    /// Ожидаемый результат: Инцидент и решение должны быть связаны корректно
    /// Тестовые данные: Решение и связанный инцидент
    /// </summary>
    [Fact]
    public async Task Database_AddIncidentWithDecision_ShouldEstablishRelationship()
    {
        // Arrange - создаем контекст, решение и инцидент
        using var context = CreateInMemoryContext();
        
        var decision = new Decision
        {
            DecisionName = "Отказано в возбуждении дела"
        };
        context.Decisions.Add(decision);
        await context.SaveChangesAsync();

        var incident = new Incident
        {
            IncidentDate = DateTime.Now,
            Description = "Мелкое хулиганство",
            DecisionId = decision.DecisionId,
            RegNumber = "INC-TEST-001"
        };
        context.Incidents.Add(incident);
        await context.SaveChangesAsync();

        // Act - загружаем инцидент с решением
        var savedIncident = await context.Incidents
            .Include(i => i.Decision)
            .FirstOrDefaultAsync(i => i.RegNumber == "INC-TEST-001");

        // Assert - проверяем связь
        Assert.NotNull(savedIncident);
        Assert.NotNull(savedIncident.Decision);
        Assert.Equal("Отказано в возбуждении дела", savedIncident.Decision.DecisionName);
        Assert.Equal(decision.DecisionId, savedIncident.DecisionId);
    }

    /// <summary>
    /// Тест 3: Проверка связи многие-ко-многим (Person-Incident через PersonIncident)
    /// Ожидаемый результат: Персона и инцидент должны быть связаны через промежуточную таблицу
    /// Тестовые данные: Персона, инцидент, роль и связь между ними
    /// </summary>
    [Fact]
    public async Task Database_LinkPersonToIncident_ShouldCreateManyToManyRelationship()
    {
        // Arrange - создаем контекст и все необходимые сущности
        using var context = CreateInMemoryContext();

        // Создаем персону
        var person = new Person
        {
            LastName = "Сидоров",
            FirstName = "Сидор",
            RegNumber = "PER-001"
        };
        context.People.Add(person);

        // Создаем инцидент
        var incident = new Incident
        {
            Description = "Кража",
            IncidentDate = DateTime.Now,
            RegNumber = "INC-001"
        };
        context.Incidents.Add(incident);

        // Создаем роль
        var role = new IncidentRole
        {
            IncidentRoleName = "Подозреваемый"
        };
        context.IncidentRoles.Add(role);

        await context.SaveChangesAsync();

        // Act - создаем связь между персоной и инцидентом
        var personIncident = new PersonIncident
        {
            PersonId = person.PersonId,
            IncidentId = incident.IncidentId,
            IncidentRoleId = role.IncidentRoleId
        };
        context.PersonIncidents.Add(personIncident);
        await context.SaveChangesAsync();

        // Assert - проверяем связь
        var savedLink = await context.PersonIncidents
            .Include(pi => pi.Person)
            .Include(pi => pi.Incident)
            .Include(pi => pi.IncidentRole)
            .FirstOrDefaultAsync();

        Assert.NotNull(savedLink);
        Assert.Equal("Сидоров", savedLink.Person?.LastName);
        Assert.Equal("Кража", savedLink.Incident?.Description);
        Assert.Equal("Подозреваемый", savedLink.IncidentRole?.IncidentRoleName);
    }

    /// <summary>
    /// Тест 4: Проверка обновления данных персоны
    /// Ожидаемый результат: Изменения должны быть сохранены в базе данных
    /// Тестовые данные: Существующая персона с обновленными данными
    /// </summary>
    [Fact]
    public async Task Database_UpdatePerson_ShouldPersistChanges()
    {
        // Arrange - создаем и сохраняем персону
        using var context = CreateInMemoryContext();
        var person = new Person
        {
            LastName = "Старая Фамилия",
            FirstName = "Старое Имя",
            RegNumber = "UPD-001"
        };
        context.People.Add(person);
        await context.SaveChangesAsync();

        // Act - обновляем данные персоны
        person.LastName = "Новая Фамилия";
        person.FirstName = "Новое Имя";
        person.Convictoins = 1;
        await context.SaveChangesAsync();

        // Assert - проверяем, что изменения сохранены
        var updatedPerson = await context.People.FindAsync(person.PersonId);
        Assert.NotNull(updatedPerson);
        Assert.Equal("Новая Фамилия", updatedPerson.LastName);
        Assert.Equal("Новое Имя", updatedPerson.FirstName);
        Assert.Equal(1, updatedPerson.Convictoins);
    }

    /// <summary>
    /// Тест 5: Проверка удаления инцидента
    /// Ожидаемый результат: Инцидент должен быть удален из базы данных
    /// Тестовые данные: Существующий инцидент
    /// </summary>
    [Fact]
    public async Task Database_DeleteIncident_ShouldRemoveFromDatabase()
    {
        // Arrange - создаем и сохраняем инцидент
        using var context = CreateInMemoryContext();
        var incident = new Incident
        {
            Description = "Инцидент для удаления",
            RegNumber = "DEL-001"
        };
        context.Incidents.Add(incident);
        await context.SaveChangesAsync();
        var incidentId = incident.IncidentId;

        // Act - удаляем инцидент
        context.Incidents.Remove(incident);
        await context.SaveChangesAsync();

        // Assert - проверяем, что инцидент удален
        var deletedIncident = await context.Incidents.FindAsync(incidentId);
        Assert.Null(deletedIncident);
    }

    /// <summary>
    /// Тест 6: Проверка подсчета количества инцидентов
    /// Ожидаемый результат: Должно быть возвращено правильное количество записей
    /// Тестовые данные: Несколько инцидентов в базе
    /// </summary>
    [Fact]
    public async Task Database_CountIncidents_ShouldReturnCorrectNumber()
    {
        // Arrange - создаем несколько инцидентов
        using var context = CreateInMemoryContext();
        context.Incidents.AddRange(
            new Incident { Description = "Инцидент 1", RegNumber = "CNT-001" },
            new Incident { Description = "Инцидент 2", RegNumber = "CNT-002" },
            new Incident { Description = "Инцидент 3", RegNumber = "CNT-003" }
        );
        await context.SaveChangesAsync();

        // Act - подсчитываем количество инцидентов
        var count = await context.Incidents.CountAsync();

        // Assert - проверяем количество
        Assert.Equal(3, count);
    }

    /// <summary>
    /// Тест 7: Проверка фильтрации инцидентов по дате
    /// Ожидаемый результат: Должны быть возвращены только инциденты за указанный период
    /// Тестовые данные: Инциденты с разными датами
    /// </summary>
    [Fact]
    public async Task Database_FilterIncidentsByDate_ShouldReturnMatchingRecords()
    {
        // Arrange - создаем инциденты с разными датами
        using var context = CreateInMemoryContext();
        var targetDate = new DateTime(2024, 12, 17);
        
        context.Incidents.AddRange(
            new Incident { Description = "Старый инцидент", IncidentDate = new DateTime(2024, 1, 1) },
            new Incident { Description = "Целевой инцидент 1", IncidentDate = targetDate },
            new Incident { Description = "Целевой инцидент 2", IncidentDate = targetDate },
            new Incident { Description = "Будущий инцидент", IncidentDate = new DateTime(2025, 1, 1) }
        );
        await context.SaveChangesAsync();

        // Act - фильтруем по дате
        var filteredIncidents = await context.Incidents
            .Where(i => i.IncidentDate == targetDate)
            .ToListAsync();

        // Assert - проверяем результат фильтрации
        Assert.Equal(2, filteredIncidents.Count);
        Assert.All(filteredIncidents, i => Assert.Equal(targetDate, i.IncidentDate));
    }
}
