using System.Collections.Generic;

namespace BusinessServicesTest
{
    public static class TestData
    {
        static Dictionary<string, int> _data;

        static TestData()
        {
            _data = new Dictionary<string, int>();
            _data["aaa"] = 1000;
            _data["bbb"] = 1000;
        }

        public static Dictionary<string, int> Current
        {
            get
            {
                return _data;
            }
        }
    }
}
