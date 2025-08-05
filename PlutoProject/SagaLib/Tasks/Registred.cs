using System.Collections.Generic;

namespace SagaLib.Tasks
{
    public class RegisteredManager : Singleton<RegisteredManager>
    {
        public List<MultiRunTask> registered = new List<MultiRunTask>();
    }
}