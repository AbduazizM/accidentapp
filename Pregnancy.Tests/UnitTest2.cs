using SKARB.Web.Models;

namespace Pregnancy.Tests;

/// <summary>
/// Юнит-тесты для модели Incident (Инцидент)
/// Тестируемый модуль: SKARB.Web.Models.Incident
/// Цель: Проверка корректности создания и валидации данных инцидента
/// </summary>
public class IncidentModelTests
{
    /// <summary>
    /// Тест 1: Проверка создания инцидента с полными данными
    /// Ожидаемый результат: Все свойства инцидента должны быть установлены правильно
    /// Тестовые данные: Дата, описание, ID решения, регистрационный номер
    /// </summary>
    [Fact]
    public void Incident_CreateWithValidData_ShouldSetPropertiesCorrectly()
    {
        // Arrange - подготавливаем тестовые данные
        var incidentDate = new DateTime(2024, 12, 17);
        var description = "Дорожно-транспортное происшествие на перекрестке";
        var decisionId = 1;
        var regNumber = "INC-2024-001";

        // Act - создаем инцидент
        var incident = new Incident
        {
            IncidentId = 1,
            IncidentDate = incidentDate,
            Description = description,
            DecisionId = decisionId,
            RegNumber = regNumber
        };

        // Assert - проверяем корректность установленных значений
        Assert.Equal(1, incident.IncidentId);
        Assert.Equal(incidentDate, incident.IncidentDate);
        Assert.Equal(description, incident.Description);
        Assert.Equal(decisionId, incident.DecisionId);
        Assert.Equal(regNumber, incident.RegNumber);
    }

    /// <summary>
    /// Тест 2: Проверка связи инцидента с решением
    /// Ожидаемый результат: Инцидент должен корректно связываться с объектом Decision
    /// Тестовые данные: Инцидент и связанное решение
    /// </summary>
    [Fact]
    public void Incident_SetDecision_ShouldEstablishRelationship()
    {
        // Arrange - создаем инцидент и решение
        var decision = new Decision
        {
            DecisionId = 1,
            DecisionName = "Возбуждено уголовное дело"
        };

        var incident = new Incident
        {
            IncidentId = 1,
            DecisionId = decision.DecisionId,
            Decision = decision
        };

        // Act & Assert - проверяем связь
        Assert.NotNull(incident.Decision);
        Assert.Equal(decision.DecisionId, incident.DecisionId);
        Assert.Equal("Возбуждено уголовное дело", incident.Decision.DecisionName);
    }

    /// <summary>
    /// Тест 3: Проверка инициализации коллекции PersonIncidents
    /// Ожидаемый результат: Коллекция участников инцидента должна быть инициализирована
    /// Тестовые данные: Новый объект Incident
    /// </summary>
    [Fact]
    public void Incident_NewInstance_ShouldInitializePersonIncidentsCollection()
    {
        // Arrange & Act - создаем новый инцидент
        var incident = new Incident();

        // Assert - проверяем инициализацию коллекции
        Assert.NotNull(incident.PersonIncidents);
        Assert.Empty(incident.PersonIncidents);
    }

    /// <summary>
    /// Тест 4: Проверка добавления участников к инциденту
    /// Ожидаемый результат: Участники должны добавляться в коллекцию
    /// Тестовые данные: Инцидент с несколькими участниками
    /// </summary>
    [Fact]
    public void Incident_AddMultiplePersons_ShouldIncreaseCollectionCount()
    {
        // Arrange - создаем инцидент и участников
        var incident = new Incident { IncidentId = 1 };
        
        var person1 = new PersonIncident { Id = 1, IncidentId = 1, PersonId = 1 };
        var person2 = new PersonIncident { Id = 2, IncidentId = 1, PersonId = 2 };

        // Act - добавляем участников
        incident.PersonIncidents.Add(person1);
        incident.PersonIncidents.Add(person2);

        // Assert - проверяем количество участников
        Assert.Equal(2, incident.PersonIncidents.Count);
        Assert.Contains(person1, incident.PersonIncidents);
        Assert.Contains(person2, incident.PersonIncidents);
    }

    /// <summary>
    /// Тест 5: Проверка создания инцидента без решения
    /// Ожидаемый результат: Инцидент может существовать без привязанного решения
    /// Тестовые данные: Инцидент без DecisionId
    /// </summary>
    [Fact]
    public void Incident_CreateWithoutDecision_ShouldAllowNullDecision()
    {
        // Arrange & Act - создаем инцидент без решения
        var incident = new Incident
        {
            IncidentId = 1,
            Description = "Инцидент в стадии рассмотрения"
        };

        // Assert - проверяем, что решение может быть null
        Assert.Null(incident.DecisionId);
        Assert.Null(incident.Decision);
    }
}
