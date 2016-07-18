namespace ZopaQuoteEngine
{
    public interface ICsvTransformer
    {
        void TransformRow(string[] row);
    }
}
