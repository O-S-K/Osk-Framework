namespace Injector
{
    public class FromMethodAttribute : FromMethodBase
    {
        public FromMethodAttribute(string method) : base(method)
        {
        }

        protected override bool IsStatic
        {
            get
            {
                return false;
            }
        }
    }
}