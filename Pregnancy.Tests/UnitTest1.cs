using SKARB.Web.Models;

namespace Pregnancy.Tests;

/// <summary>
/// Юнит-тесты для модели Person (Персона)
/// Тестируемый модуль: SKARB.Web.Models.Person
/// Цель: Проверка корректности создания и валидации данных персоны
/// </summary>
public class PersonModelTests
{
    /// <summary>
    /// Тест 1: Проверка создания персоны с корректными данными
    /// Ожидаемый результат: Все свойства персоны должны быть установлены правильно
    /// Тестовые данные: ФИО, адрес, количество судимостей, регистрационный номер
    /// </summary>
    [Fact]
    public void Person_CreateWithValidData_ShouldSetPropertiesCorrectly()
    {
        // Arrange (Подготовка) - создаем тестовые данные
        var lastName = "Иванов";
        var firstName = "Иван";
        var middleName = "Иванович";
        var address = "г. Москва, ул. Ленина, д. 1";
        var convictions = 0;
        var regNumber = "REG-001";

        // Act (Действие) - создаем объект Person
        var person = new Person
        {
            PersonId = 1,
            LastName = lastName,
            FirstName = firstName,
            MiddleName = middleName,
            Address = address,
            Convictoins = convictions,
            RegNumber = regNumber
        };

        // Assert (Проверка) - проверяем, что все свойства установлены правильно
        Assert.Equal(1, person.PersonId);
        Assert.Equal(lastName, person.LastName);
        Assert.Equal(firstName, person.FirstName);
        Assert.Equal(middleName, person.MiddleName);
        Assert.Equal(address, person.Address);
        Assert.Equal(convictions, person.Convictoins);
        Assert.Equal(regNumber, person.RegNumber);
    }

    /// <summary>
    /// Тест 2: Проверка создания персоны с пустыми необязательными полями
    /// Ожидаемый результат: Персона должна создаваться с null значениями
    /// Тестовые данные: Только ID персоны
    /// </summary>
    [Fact]
    public void Person_CreateWithNullableFields_ShouldAllowNullValues()
    {
        // Arrange & Act - создаем персону без заполнения необязательных полей
        var person = new Person
        {
            PersonId = 2
        };

        // Assert - проверяем, что необязательные поля могут быть null
        Assert.Null(person.LastName);
        Assert.Null(person.FirstName);
        Assert.Null(person.MiddleName);
        Assert.Null(person.Address);
        Assert.Null(person.Convictoins);
        Assert.Null(person.RegNumber);
    }

    /// <summary>
    /// Тест 3: Проверка инициализации коллекции PersonIncidents
    /// Ожидаемый результат: Коллекция должна быть инициализирована и пустая
    /// Тестовые данные: Новый объект Person
    /// </summary>
    [Fact]
    public void Person_NewInstance_ShouldInitializePersonIncidentsCollection()
    {
        // Arrange & Act - создаем новую персону
        var person = new Person();

        // Assert - проверяем, что коллекция инцидентов инициализирована
        Assert.NotNull(person.PersonIncidents);
        Assert.Empty(person.PersonIncidents);
    }

    /// <summary>
    /// Тест 4: Проверка добавления инцидентов к персоне
    /// Ожидаемый результат: Инциденты должны добавляться в коллекцию
    /// Тестовые данные: Персона и связанный инцидент
    /// </summary>
    [Fact]
    public void Person_AddPersonIncident_ShouldIncreaseCollectionCount()
    {
        // Arrange - создаем персону и инцидент
        var person = new Person { PersonId = 1 };
        var personIncident = new PersonIncident
        {
            Id = 1,
            PersonId = 1,
            IncidentId = 1,
            Person = person
        };

        // Act - добавляем инцидент к персоне
        person.PersonIncidents.Add(personIncident);

        // Assert - проверяем, что инцидент добавлен
        Assert.Single(person.PersonIncidents);
        Assert.Contains(personIncident, person.PersonIncidents);
    }
}
