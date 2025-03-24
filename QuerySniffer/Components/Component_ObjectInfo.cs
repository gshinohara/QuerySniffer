using Grasshopper.Kernel;
using System;

namespace QuerySniffer.Components
{
    public class Component_ObjectInfo : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Component_ObjectInfo class.
        /// </summary>
        public Component_ObjectInfo()
          : base("Object Info", "Info",
              "",
              "QuerySniffer", "Analysis")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Object", "O", "Object to get information of", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name of the object", GH_ParamAccess.item);
            pManager.AddTextParameter("NickName", "NN", "NickName of the object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_DocumentObject obj = null;
            if (!DA.GetData("Object", ref obj)) return;

            DA.SetData("Name", obj.Name);
            DA.SetData("NickName", obj.NickName);
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
            get { return new Guid("A7041368-FC13-4135-B387-4AB0671C759B"); }
        }
    }
}