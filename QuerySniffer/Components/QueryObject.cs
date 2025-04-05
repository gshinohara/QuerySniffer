using GH_IO.Serialization;
using Grasshopper.Kernel;
using QuerySniffer.Components.Attributes;
using QuerySniffer.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuerySniffer.Components
{
    public class QueryObject : GH_Component
    {
        public List<Guid> ConnectedObjects { get; } = new List<Guid>();

        public QueryObject() : base("Query Object", "QO", "Query Object", "QuerySniffer", "Pick")
        {
        }

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            // No input parameters
        }

        protected override void RegisterOutputParams(GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter(default, default, default, GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            DA.SetDataList(0, ConnectedObjects.Select(id => new GrasshopperObject(OnPingDocument().FindObject(id, true))));
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
            if (ConnectedObjects.Any(id => id == sender.InstanceGuid))
                ExpireSolution(true);
        }

        public override Guid ComponentGuid => new Guid("ECE7231E-02C9-4352-AA18-86B193F92361");

        public override void CreateAttributes()
        {
            Attributes = new QueryObjectAttributes(this);
        }

        public override bool Write(GH_IWriter writer)
        {
            GH_IWriter chunk = writer.CreateChunk("QueryObject");

            chunk.SetInt32("Count", ConnectedObjects.Count);

            for (int i = 0; i < ConnectedObjects.Count; i++)
                chunk.SetGuid("Object", i, ConnectedObjects[i]);

            return base.Write(writer);
        }

        public override bool Read(GH_IReader reader)
        {
            GH_IReader chunk = reader.FindChunk("QueryObject");

            ConnectedObjects.Clear();
            for (int i = 0; i < chunk.GetInt32("Count"); i++)
                ConnectedObjects.Add(chunk.GetGuid("Object", i));

            return base.Read(reader);
        }
    }
}