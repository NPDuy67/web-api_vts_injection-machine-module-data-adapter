namespace InjectionMoldingMachineDataAdapter.Application.Dtos;
public class QueryResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalItems { get; set; } = 0;
}
