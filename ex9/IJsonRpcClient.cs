namespace jsonrpc
{
    public interface IJsonRpcClient
    {
        public Result Invoke<Result>(string method, object @params, int id) where Result : class;
    }
}
