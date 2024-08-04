namespace FontParser.Tables.AdvancedLayout.JustificationTable
{
    public class JstfPriority
    {
        //JstfPriority table

        //Type     Name                    Description
        //Offset16 shrinkageEnableGSUB     Offset to shrinkage-enable JstfGSUBModList table, from beginning of JstfPriority table(may be NULL)
        //Offset16 shrinkageDisableGSUB    Offset to shrinkage-disable JstfGSUBModList table, from beginning of JstfPriority table(may be NULL)
        public ushort shrinkageEnableGSUB;

        public ushort shrinkageDisableGSUB;

        //Offset16 shrinkageEnableGPOS     Offset to shrinkage-enable JstfGPOSModList table, from beginning of JstfPriority table(may be NULL)
        //Offset16 shrinkageDisableGPOS    Offset to shrinkage-disable JstfGPOSModList table, from beginning of JstfPriority table(may be NULL)

        public ushort shrinkageEnableGPOS;
        public ushort shrinkageDisableGPOS;

        //Offset16 shrinkageJstfMax        Offset to shrinkage JstfMax table, from beginning of JstfPriority table(may be NULL)
        public ushort shrinkageJstfMax;

        //Offset16 extensionEnableGSUB     Offset to extension-enable JstfGSUBModList table, from beginnning of JstfPriority table(may be NULL)
        //Offset16 extensionDisableGSUB    Offset to extension-disable JstfGSUBModList table, from beginning of JstfPriority table(may be NULL)

        public ushort extensionEnableGSUB;
        public ushort extensionDisableGSUB;

        //Offset16 extensionEnableGPOS     Offset to extension-enable JstfGPOSModList table, from beginning of JstfPriority table(may be NULL)
        //Offset16 extensionDisableGPOS    Offset to extension-disable JstfGPOSModList table, from beginning of JstfPriority table(may be NULL)

        public ushort extensionEnableGPOS;
        public ushort extensionDisableGPOS;

        //Offset16 extensionJstfMax        Offset to extension JstfMax table, from beginning of JstfPriority table(may be NULL)
        public ushort extensionJstfMax;
    }
}
