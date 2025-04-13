using Grasshopper.Kernel;
using QuerySniffer.Analysis.DAG;
using QuerySniffer.Parameters;
using System;

namespace QuerySniffer.Components
{
    public class Component_GH_Graph_Visualizer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Component_GH_Graph_Visualizer class.
        /// </summary>
        public Component_GH_Graph_Visualizer()
          : base("Graph Visualizer", "DAG Viz",
              "Visualizes graphs in Grasshopper",
              "QuerySniffer", "DAG")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddParameter(new Param_Graph(), "Graph", "G", "The graph to visualize", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Graph graph = null;
            if (!DA.GetData("Graph", ref graph)) return;

            QuerySnifferPriority.VisGraphs.Clear();
            QuerySnifferPriority.VisGraphs.Add(graph);
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
            get { return new Guid("D3D57306-CEAA-43CC-9F15-8E3D3A7E5C1B"); }
        }
    }
}