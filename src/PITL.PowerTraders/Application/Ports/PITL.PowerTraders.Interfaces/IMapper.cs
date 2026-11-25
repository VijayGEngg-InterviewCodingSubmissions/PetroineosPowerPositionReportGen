namespace PITL.PowerTraders.Interfaces
{
    public interface IMapper<TSource, TResult>
    {
        TResult MapToDTO(TSource source);

        IEnumerable<TResult> MapListToDTOList(IEnumerable<TSource> source);
    }
}
