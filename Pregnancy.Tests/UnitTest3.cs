using SKARB.Web.Methods;

namespace Pregnancy.Tests;

/// <summary>
/// Юнит-тесты для класса PaginatedList (Пагинация)
/// Тестируемый модуль: SKARB.Web.Methods.PaginatedList
/// Цель: Проверка корректности работы пагинации списков
/// </summary>
public class PaginatedListTests
{
    /// <summary>
    /// Тест 1: Проверка создания пагинированного списка
    /// Ожидаемый результат: Список должен содержать правильное количество элементов и страниц
    /// Тестовые данные: 10 элементов, размер страницы 3, первая страница
    /// </summary>
    [Fact]
    public void PaginatedList_Create_ShouldCalculateTotalPagesCorrectly()
    {
        // Arrange - создаем тестовые данные (10 элементов)
        var items = new List<int> { 1, 2, 3 }; // Первая страница из 3 элементов
        var totalCount = 10; // Всего элементов
        var pageIndex = 1; // Первая страница
        var pageSize = 3; // По 3 элемента на странице

        // Act - создаем пагинированный список
        var paginatedList = new PaginatedList<int>(items, totalCount, pageIndex, pageSize);

        // Assert - проверяем расчеты
        Assert.Equal(1, paginatedList.PageIndex); // Текущая страница = 1
        Assert.Equal(4, paginatedList.TotalPages); // Всего страниц = 10/3 = 4 (округление вверх)
        Assert.Equal(3, paginatedList.Count); // На странице 3 элемента
    }

    /// <summary>
    /// Тест 2: Проверка наличия предыдущей страницы
    /// Ожидаемый результат: На первой странице не должно быть предыдущей страницы
    /// Тестовые данные: Первая страница списка
    /// </summary>
    [Fact]
    public void PaginatedList_FirstPage_ShouldNotHavePreviousPage()
    {
        // Arrange - создаем первую страницу
        var items = new List<string> { "Item1", "Item2" };
        var paginatedList = new PaginatedList<string>(items, 10, 1, 2);

        // Act & Assert - проверяем отсутствие предыдущей страницы
        Assert.False(paginatedList.HasPreviousPage);
    }

    /// <summary>
    /// Тест 3: Проверка наличия следующей страницы
    /// Ожидаемый результат: На последней странице не должно быть следующей страницы
    /// Тестовые данные: Последняя страница списка
    /// </summary>
    [Fact]
    public void PaginatedList_LastPage_ShouldNotHaveNextPage()
    {
        // Arrange - создаем последнюю страницу (страница 3 из 3)
        var items = new List<string> { "Item9", "Item10" };
        var totalCount = 10;
        var pageIndex = 3; // Последняя страница
        var pageSize = 4; // 10 элементов / 4 = 3 страницы
        
        var paginatedList = new PaginatedList<string>(items, totalCount, pageIndex, pageSize);

        // Act & Assert - проверяем отсутствие следующей страницы
        Assert.False(paginatedList.HasNextPage);
        Assert.Equal(3, paginatedList.TotalPages);
    }

    /// <summary>
    /// Тест 4: Проверка средней страницы
    /// Ожидаемый результат: Средняя страница должна иметь и предыдущую, и следующую страницы
    /// Тестовые данные: Вторая страница из трех
    /// </summary>
    [Fact]
    public void PaginatedList_MiddlePage_ShouldHaveBothPreviousAndNextPages()
    {
        // Arrange - создаем среднюю страницу (страница 2 из 3)
        var items = new List<string> { "Item5", "Item6", "Item7", "Item8" };
        var paginatedList = new PaginatedList<string>(items, 12, 2, 4);

        // Act & Assert - проверяем наличие обеих навигационных ссылок
        Assert.True(paginatedList.HasPreviousPage); // Есть предыдущая страница
        Assert.True(paginatedList.HasNextPage); // Есть следующая страница
        Assert.Equal(2, paginatedList.PageIndex); // Текущая страница = 2
        Assert.Equal(3, paginatedList.TotalPages); // Всего 3 страницы
    }

    /// <summary>
    /// Тест 5: Проверка пустого списка
    /// Ожидаемый результат: Пустой список должен иметь 0 страниц
    /// Тестовые данные: Пустой список элементов
    /// </summary>
    [Fact]
    public void PaginatedList_EmptyList_ShouldHaveZeroPages()
    {
        // Arrange - создаем пустой список
        var items = new List<string>();
        var paginatedList = new PaginatedList<string>(items, 0, 1, 10);

        // Act & Assert - проверяем пустой список
        Assert.Empty(paginatedList);
        Assert.Equal(0, paginatedList.TotalPages);
        Assert.False(paginatedList.HasPreviousPage);
        Assert.False(paginatedList.HasNextPage);
    }

    /// <summary>
    /// Тест 6: Проверка расчета страниц при неполной последней странице
    /// Ожидаемый результат: Должно быть правильное округление количества страниц
    /// Тестовые данные: 7 элементов по 3 на странице = 3 страницы
    /// </summary>
    [Fact]
    public void PaginatedList_PartialLastPage_ShouldRoundUpTotalPages()
    {
        // Arrange - 7 элементов, по 3 на странице
        var items = new List<int> { 1, 2, 3 };
        var paginatedList = new PaginatedList<int>(items, 7, 1, 3);

        // Act & Assert - проверяем округление вверх (7/3 = 2.33 → 3 страницы)
        Assert.Equal(3, paginatedList.TotalPages);
    }
}
