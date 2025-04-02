using Grasshopper.Kernel;
using QuerySniffer.Analysis.DAG;
using QuerySniffer.Parameters;
using QuerySniffer.Types;
using System;
using System.Linq;

namespace QuerySniffer.Components
{
    public class Component_DeconstructGraph : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Component_DeconstructGraph class.
        /// </summary>
        public Component_DeconstructGraph()
          : base("Deconstruct Graph", "Decon",
              "Deconstruct a graph to get nodes and their depth.",
              "QuerySniffer", "Analysis")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Graph(), "Graph", "G", "Graph to be deconstructed", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Nodes", "N", "Nodes in graph", GH_ParamAccess.list);
            pManager.AddIntegerParameter("Depths", "D", "Depths of nodes", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Graph graph = null;
            if (!DA.GetData("Graph", ref graph)) return;

            Func<INode, GrasshopperObject> cast = node =>
            {
                GrasshopperObject grasshopperObject = new GrasshopperObject();
                grasshopperObject.CastFrom(node);
                return grasshopperObject;
            };

            DA.SetDataList("Nodes", graph.Nodes.Select(node => cast(node)));
            DA.SetDataList("Depths", graph.Nodes.Select(node => node.Depth));
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("CE8A756D-6254-4D8C-853A-34DD756544C1"); }
        }
    }
}