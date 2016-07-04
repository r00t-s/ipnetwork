namespace System.Net.IPNetwork.ConsoleApplication
{
    public class ArgParsed
    {
        public int Arg;
        private event ArgParsedDelegate OnArgParsed;
        public delegate void ArgParsedDelegate(ProgramContext ac, string arg);

        public void Run(ProgramContext ac, string arg)
        {
            OnArgParsed?.Invoke(ac, arg);
        }

        public ArgParsed(int arg, ArgParsedDelegate onArgParsed)
        {
            Arg = arg;
            OnArgParsed += onArgParsed;
        }



    }
}
