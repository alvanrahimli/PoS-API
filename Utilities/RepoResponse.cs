namespace StarDMS.Utilities
{
    public class RepoResponse<T>
    {
        public bool IsSucces { get; set; }
        public T Content { get; set; }
        public string Message { get; set; }
    }
}