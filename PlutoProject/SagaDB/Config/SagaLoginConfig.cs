using System;
using SagaDB.Item;

namespace SagaDB.Config
{
    

    [Serializable]
    public class StartItem
    {
        public byte Count;
        public uint ItemID;
        public ContainerType Slot;
    }
}