namespace BlazorTest.Entities
{
    public class MyEntity
    {
        public string Id { get; set; }
        public string StringProp { get; set; }
        public int IntProp { get; set; }
        public List<int> IntList { get; set; }
        public InnerClass InnerProperty { get; set; }

        public class InnerClass
        {
            public string InnerString { get; set; }
            public List<string> StringList { get; set; }

        }
    }
}
