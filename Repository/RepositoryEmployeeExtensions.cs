using Domains;

namespace Repository;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
    {
        return employees.Where(e => e.Age >= minAge && e.Age <= maxAge);
    }

    public static IQueryable<Employee> Sarch(this IQueryable<Employee> employees, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString)) return employees;
        var lowerCaseTerm = searchString.Trim().ToLower();
        return employees.Where(x => x.Name.ToLower().Contains(lowerCaseTerm));
    }
}