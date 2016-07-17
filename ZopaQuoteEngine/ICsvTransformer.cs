namespace ZopaQuoteEngine
{
    internal interface ICsvTransformer
    {
        void TransformRow(string[] row);
    }
}
