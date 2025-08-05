using System.Collections.Generic;

namespace SagaLib
{
    public class RegisteredManager : Singleton<RegisteredManager>
    {
        public List<MultiRunTask> registered = new List<MultiRunTask>();
    }
}