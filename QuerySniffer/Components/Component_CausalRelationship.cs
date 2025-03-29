using Grasshopper.Kernel;
using QuerySniffer.Analysis.DAG;
using QuerySniffer.Types;
using System;
using System.Collections.Generic;

namespace QuerySniffer.Components
{
    public class Component_CausalRelationship : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Component_CausalRelationship class.
        /// </summary>
        public Component_CausalRelationship()
          : base("Causal Relationship", "Causal",
              "Analyse the grasshopper algorythm in the DAG theoty.",
              "QuerySniffer", "Analysis")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Object", "O", "Object of the start point on DAG", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Left", "L", "Object following in the left direction", GH_ParamAccess.list);
            pManager.AddGenericParameter("Right", "R", "Object following in the right direction", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            IGH_DocumentObject obj = null;
            if (!DA.GetData("Object", ref obj)) return;

            List<GrasshopperObject> lefts = new List<GrasshopperObject>();
            List<GrasshopperObject> rights = new List<GrasshopperObject>();

            foreach (INode node in NodeExplorer.GetAllLeftNodes(obj.GetTopLevelNode()))
            {
                GrasshopperObject grasshopperObject = new GrasshopperObject();
                grasshopperObject.CastFrom(node);
                lefts.Add(grasshopperObject);
            }

            foreach (INode node in NodeExplorer.GetAllRightNodes(obj.GetTopLevelNode()))
            {
                GrasshopperObject grasshopperObject = new GrasshopperObject();
                grasshopperObject.CastFrom(node);
                rights.Add(grasshopperObject);
            }

            DA.SetDataList("Left", lefts);
            DA.SetDataList("Right", rights);
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
            get { return new Guid("CDE0FFD9-D27B-499C-B79C-4BDE9CBC50A5"); }
        }
    }
}