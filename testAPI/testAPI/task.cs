namespace testAPI
{
    public enum Stat
    {
        due,
        onProgress,
        done
    }
    public class task
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Stat Status { get; set; }
    }
}
