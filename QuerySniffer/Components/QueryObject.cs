using Grasshopper.Kernel;
using QuerySniffer.Components.Attributes;
using QuerySniffer.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuerySniffer.Components
{
    public class QueryObject : GH_Param<GrasshopperObject>
    {
        public List<GrasshopperObject> ConnectedObjects { get; } = new List<GrasshopperObject>();

        public QueryObject() : base("Query Object", "QO", "Query Object", "QuerySniffer", "Pick", GH_ParamAccess.item)
        {
            Optional = true;
        }

        public override void AddedToDocument(GH_Document document)
        {
            base.AddedToDocument(document);

            document.ObjectsAdded += Document_ObjectsAdded;

            foreach (var obj in document.Objects)
                obj.ObjectChanged += OnObjectChanged;
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            base.RemovedFromDocument(document);

            document.ObjectsAdded -= Document_ObjectsAdded;

            foreach (var obj in document.Objects)
                obj.ObjectChanged -= OnObjectChanged;
        }

        private void Document_ObjectsAdded(object sender, GH_DocObjectEventArgs e)
        {
            foreach (var obj in e.Objects)
                obj.ObjectChanged += OnObjectChanged;
        }

        private void OnObjectChanged(IGH_DocumentObject sender, GH_ObjectChangedEventArgs e)
        {
            if (ConnectedObjects.Any(obj => obj.Value == sender))
                ExpireSolution(true);
        }

        public override void ExpireSolution(bool recompute)
        {
            base.ExpireSolution(recompute);

            VolatileData.Clear();
            AddVolatileDataList(new Grasshopper.Kernel.Data.GH_Path(0), ConnectedObjects);

            foreach (var param in Recipients)
                param.ExpireSolution(true);
        }

        public override Guid ComponentGuid => new Guid("ECE7231E-02C9-4352-AA18-86B193F92361");

        public override void CreateAttributes()
        {
            Attributes = new QueryObjectAttributes(this);
        }
    }
}