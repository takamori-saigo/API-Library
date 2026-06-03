namespace restor;

public abstract class RequestParameters
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;

    private int _pageSize = 10;

    public int PageSize
    {
        get
        {
            return _pageSize;
        }

        set
        {
            _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}

public class EmployeeParameters : RequestParameters
{
    public uint MinAge { get; set; }
    public uint MaxAge { get; set; } = int.MaxValue;
    public bool ValidAgeRange => MaxAge > MinAge;
}