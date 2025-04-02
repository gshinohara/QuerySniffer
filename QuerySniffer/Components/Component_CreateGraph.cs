using Grasshopper.Kernel;
using QuerySniffer.Analysis.DAG;
using QuerySniffer.Parameters;
using System;

namespace QuerySniffer.Components
{
    public class Component_CreateGraph : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateGraph class.
        /// </summary>
        public Component_CreateGraph()
          : base("Create Graph", "Graph",
              "Create a directed acyclic graph derived from an object.",
              "QuerySniffer", "Analysis")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Object", "O", "Object of a start point", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddParameter(new Param_Graph(), "Graph", "G", "Created graph", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_DocumentObject documentObject = null;
            if (!DA.GetData("Object", ref documentObject)) return;

            Graph graph = Graph.Create(documentObject);
            DA.SetData("Graph", graph);
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
            get { return new Guid("41DDC8D3-22AB-4692-AF6C-0028DFC9C390"); }
        }
    }
}