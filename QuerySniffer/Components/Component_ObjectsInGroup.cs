using Grasshopper.Kernel;
using QuerySniffer.Types;
using System;
using System.Linq;

namespace QuerySniffer.Components
{
    public class Component_ObjectsInGroup : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Component_ObjectsInGroup class.
        /// </summary>
        public Component_ObjectsInGroup()
          : base("Objects In Group", "Group",
              "",
              "QuerySniffer", "Analysis")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Group", "G", "Group to query", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Objects", "O", "Objects in the group", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Grasshopper.Kernel.Special.GH_Group group = null;
            if (!DA.GetData("Group", ref group)) return;

            DA.SetDataList("Objects", group.Objects().Select(obj => new GrasshopperObject(obj)));
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
            get { return new Guid("83B0D7A3-44B7-4074-8172-43B7C53530D9"); }
        }
    }
}