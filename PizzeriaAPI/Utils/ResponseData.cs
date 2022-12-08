namespace PizzeriaAPI.Utils
{
    public class ResponseData<T>
    {
        public bool Success { get; set; }
        public ResponseCode? Reason { get; set; }
        public T? Value { get; set; }
    }
}
