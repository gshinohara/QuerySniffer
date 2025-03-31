using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using QuerySniffer.Analysis.DAG;

public static class Utility
{
    private static IGH_DocumentObject GetTopLevel(IGH_DocumentObject obj)
    {
        switch (obj.Attributes)
        {
            case GH_LinkedParamAttributes linkedParamAtt:
                return linkedParamAtt.GetTopLevel.DocObject;
            case GH_FloatingParamAttributes floatingParamAtt:
                return obj;
            default:
                break;
        }

        switch (obj)
        {
            case IGH_Param _:
            case GH_Component _:
                return obj;
            default:
                throw new System.NotImplementedException();
        }
    }

    private static INode GetNode(IGH_DocumentObject obj)
    {
        switch (obj)
        {
            case GH_Component component:
                return new ComponentNode(component);
            case IGH_Param param:
                return new ParameterNode(param);
            default:
                throw new System.NotImplementedException();
        }
    }

    public static INode GetTopLevelNode(this IGH_DocumentObject obj)
    {
        return GetNode(GetTopLevel(obj));
    }
}