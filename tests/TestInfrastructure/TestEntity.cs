namespace FluentValidation.HttpExtensions.TestInfrastructure
{
    public class TestEntity
    {
        public bool IsForbidden { get; set; }
        public bool IsNotFound { get; set; }
        public bool IsMethodNotAllowed { get; set; }
        public bool IsNotAcceptable { get; set; }
        public bool IsConflict { get; set; }
        public bool IsGone { get; set; }
        public bool IsLocked { get; set; }
        public bool IsValid { get; set; }
    }

    public class NotFoundEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
    }
}
