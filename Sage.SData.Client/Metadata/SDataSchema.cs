﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;

namespace Sage.SData.Client.Metadata
{
    public class SDataSchema : SDataSchemaObject
    {
        private IList<XmlSchemaImport> _imports;
        private IList<XmlQualifiedName> _namespaces;
        private KeyedObjectCollection<SDataSchemaType> _types;

        public SDataSchema()
        {
        }

        public SDataSchema(string targetNamespace)
        {
            TargetNamespace = targetNamespace;
            ElementFormDefault = XmlSchemaForm.Qualified;
        }

        public string Id { get; set; }
        public string TargetNamespace { get; set; }
        public string Version { get; set; }
        public XmlSchemaForm ElementFormDefault { get; set; }

        public override IEnumerable<SDataSchemaObject> Children
        {
            get { return _types.Cast<SDataSchemaObject>(); }
        }

        public IList<XmlQualifiedName> Namespaces
        {
            get { return _namespaces ?? (_namespaces = new List<XmlQualifiedName>()); }
        }

        public IList<XmlSchemaImport> Imports
        {
            get { return _imports ?? (_imports = new List<XmlSchemaImport>()); }
        }

        public KeyedObjectCollection<SDataSchemaType> Types
        {
            get { return _types ?? (_types = new KeyedObjectCollection<SDataSchemaType>(this, type => type.Name)); }
        }

        public KeyedEnumerable<string, SDataSchemaSimpleType> SimpleTypes
        {
            get { return new KeyedEnumerable<string, SDataSchemaSimpleType>(Types.OfType<SDataSchemaSimpleType>(), type => type.Name); }
        }

        public KeyedEnumerable<string, SDataSchemaEnumType> EnumTypes
        {
            get { return new KeyedEnumerable<string, SDataSchemaEnumType>(Types.OfType<SDataSchemaEnumType>(), type => type.Name); }
        }

        public KeyedEnumerable<string, SDataSchemaComplexType> ComplexTypes
        {
            get { return new KeyedEnumerable<string, SDataSchemaComplexType>(Types.OfType<SDataSchemaComplexType>(), type => type.Name); }
        }

        public KeyedEnumerable<string, SDataSchemaResourceType> ResourceTypes
        {
            get { return new KeyedEnumerable<string, SDataSchemaResourceType>(Types.OfType<SDataSchemaResourceType>(), type => type.ElementName); }
        }

        public KeyedEnumerable<string, SDataSchemaServiceOperationType> ServiceOperationTypes
        {
            get { return new KeyedEnumerable<string, SDataSchemaServiceOperationType>(Types.OfType<SDataSchemaServiceOperationType>(), type => type.ElementName); }
        }

        public KeyedEnumerable<string, SDataSchemaNamedQueryType> NamedQueryTypes
        {
            get { return new KeyedEnumerable<string, SDataSchemaNamedQueryType>(Types.OfType<SDataSchemaNamedQueryType>(), type => type.ElementName); }
        }

        public static SDataSchema Read(Stream stream)
        {
            return BuildReadSchema(XmlSchema.Read(stream, null));
        }

        public static SDataSchema Read(TextReader reader)
        {
            return BuildReadSchema(XmlSchema.Read(reader, null));
        }

        public static SDataSchema Read(XmlReader reader)
        {
            return BuildReadSchema(XmlSchema.Read(reader, null));
        }

        private static SDataSchema BuildReadSchema(XmlSchema xmlSchema)
        {
            var schema = new SDataSchema();
            schema.Read(xmlSchema);
            return schema;
        }

        public void Write(Stream stream)
        {
            BuildWriteSchema().Write(stream);
        }

        public void Write(TextWriter writer)
        {
            BuildWriteSchema().Write(writer);
        }

        public void Write(XmlWriter writer)
        {
            BuildWriteSchema().Write(writer);
        }

        private XmlSchema BuildWriteSchema()
        {
            var xmlSchema = new XmlSchema();
            Write(xmlSchema);
            return xmlSchema;
        }

        private void Read(XmlSchemaObject obj)
        {
            var xmlSchema = (XmlSchema) obj;
            Id = xmlSchema.Id;
            TargetNamespace = xmlSchema.TargetNamespace;
            Version = xmlSchema.Version;
            ElementFormDefault = xmlSchema.ElementFormDefault;

            foreach (var ns in xmlSchema.Namespaces.ToArray())
            {
                Namespaces.Add(ns);
            }

            foreach (var import in xmlSchema.Includes.OfType<XmlSchemaImport>())
            {
                Imports.Add(import);
            }

            var elements = xmlSchema.Items.OfType<XmlSchemaElement>().ToDictionary(element => element.SchemaTypeName);
            var types = new Dictionary<XmlQualifiedName, SDataSchemaType>();
            var lists = new Dictionary<XmlQualifiedName, XmlSchemaComplexType>();

            foreach (var item in xmlSchema.Items.OfType<XmlSchemaType>())
            {
                SDataSchemaType type;
                var qualifiedName = new XmlQualifiedName(item.Name, TargetNamespace);

                if (item is XmlSchemaComplexType)
                {
                    var xmlComplexType = (XmlSchemaComplexType) item;

                    if (xmlComplexType.Particle == null || xmlComplexType.Particle is XmlSchemaAll)
                    {
                        XmlSchemaElement element;

                        if (elements.TryGetValue(qualifiedName, out element))
                        {
                            var roleAttr = element.UnhandledAttributes != null
                                               ? element.UnhandledAttributes.FirstOrDefault(attr => attr.NamespaceURI == SmeNamespaceUri && attr.LocalName == "role")
                                               : null;

                            if (roleAttr == null)
                            {
                                throw new InvalidOperationException(string.Format("Role attribute on top level element '{0}' not found", element.Name));
                            }

                            switch (roleAttr.Value)
                            {
                                case "resourceKind":
                                    type = new SDataSchemaResourceType();
                                    break;
                                case "serviceOperation":
                                    type = new SDataSchemaServiceOperationType();
                                    break;
                                case "query":
                                    type = new SDataSchemaNamedQueryType();
                                    break;
                                default:
                                    throw new InvalidOperationException(string.Format("Unexpected role attribute value '{0}' on top level element '{1}'", roleAttr.Value, element.Name));
                            }

                            type.Read(element);
                            elements.Remove(qualifiedName);
                        }
                        else
                        {
                            type = new SDataSchemaComplexType();
                        }
                    }
                    else if (xmlComplexType.Particle is XmlSchemaSequence)
                    {
                        var sequence = (XmlSchemaSequence) xmlComplexType.Particle;

                        if (sequence.Items.Count != 1)
                        {
                            throw new InvalidOperationException(string.Format("Particle on list complex type '{0}' does not contain exactly one element", xmlComplexType.Name));
                        }

                        var element = sequence.Items[0] as XmlSchemaElement;

                        if (element == null)
                        {
                            throw new InvalidOperationException(string.Format("Unexpected sequence item type '{0}' on list complex type '{1}'", sequence.Items[0].GetType(), xmlComplexType.Name));
                        }

                        SDataSchemaType complexType;

                        if (types.TryGetValue(element.SchemaTypeName, out complexType))
                        {
                            complexType.ListName = xmlComplexType.Name;
                            complexType.ListItemName = element.Name;
                            complexType.ListAnyAttribute = xmlComplexType.AnyAttribute;
                            types.Remove(element.SchemaTypeName);
                        }
                        else
                        {
                            lists.Add(element.SchemaTypeName, xmlComplexType);
                        }

                        continue;
                    }
                    else if (xmlComplexType.Particle is XmlSchemaChoice)
                    {
                        type = new SDataSchemaChoiceType();
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("Unexpected particle type '{0}' on complex type '{1}'", xmlComplexType.Particle.GetType(), xmlComplexType.Name));
                    }
                }
                else if (item is XmlSchemaSimpleType)
                {
                    var simpleType = (XmlSchemaSimpleType) item;

                    if (simpleType.Content == null)
                    {
                        throw new InvalidOperationException(string.Format("Missing content on simple type '{0}'", simpleType.Name));
                    }

                    var restriction = simpleType.Content as XmlSchemaSimpleTypeRestriction;

                    if (restriction == null)
                    {
                        throw new InvalidOperationException(string.Format("Unexpected content type '{0}' on simple type '{1}'", simpleType.Content.GetType(), simpleType.Name));
                    }

                    if (restriction.Facets.Cast<XmlSchemaObject>().All(facet => facet is XmlSchemaEnumerationFacet))
                    {
                        type = new SDataSchemaEnumType();
                    }
                    else
                    {
                        type = new SDataSchemaSimpleType();
                    }
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Unexpected item type '{0}'", item.GetType()));
                }

                XmlSchemaComplexType complexList;
                if (lists.TryGetValue(qualifiedName, out complexList))
                {
                    var sequence = (XmlSchemaSequence) complexList.Particle;
                    var itemElement = (XmlSchemaElement) sequence.Items[0];

                    type.ListName = complexList.Name;
                    type.ListItemName = itemElement.Name;
                    type.ListAnyAttribute = complexList.AnyAttribute;
                    lists.Remove(qualifiedName);
                }
                else
                {
                    types.Add(qualifiedName, type);
                }

                type.Read(item);
                Types.Add(type);
            }

            Compile();
        }

        private void Write(XmlSchemaObject obj)
        {
            var xmlSchema = (XmlSchema) obj;
            xmlSchema.Id = Id;
            xmlSchema.TargetNamespace = TargetNamespace;
            xmlSchema.Version = Version;
            xmlSchema.ElementFormDefault = ElementFormDefault;

            var hasSme = false;
            var hasXs = false;
            var hasDefault = false;

            foreach (var ns in Namespaces)
            {
                xmlSchema.Namespaces.Add(ns.Name, ns.Namespace);
                hasSme |= ns.Namespace == SmeNamespaceUri;
                hasXs |= ns.Namespace == XmlSchema.Namespace;
                hasDefault |= ns.Namespace == TargetNamespace;
            }

            foreach (var import in Imports)
            {
                xmlSchema.Includes.Add(import);
            }

            if (!hasSme)
            {
                xmlSchema.Namespaces.Add("sme", SmeNamespaceUri);
            }

            if (!hasXs)
            {
                xmlSchema.Namespaces.Add("xs", XmlSchema.Namespace);
            }

            if (!hasDefault)
            {
                xmlSchema.Namespaces.Add(string.Empty, TargetNamespace);
            }

            foreach (var type in Types)
            {
                if (type is SDataSchemaComplexType)
                {
                    var complexType = (SDataSchemaComplexType) type;

                    if (type is SDataSchemaTopLevelType)
                    {
                        var element = new XmlSchemaElement {SchemaTypeName = complexType.QualifiedName};
                        type.Write(element);
                        xmlSchema.Items.Add(element);
                    }

                    var xmlComplexType = new XmlSchemaComplexType();
                    type.Write(xmlComplexType);
                    xmlSchema.Items.Add(xmlComplexType);

                    if (complexType.ListName != null)
                    {
                        xmlComplexType = new XmlSchemaComplexType
                                         {
                                             Name = complexType.ListName,
                                             AnyAttribute = complexType.ListAnyAttribute,
                                             Particle = new XmlSchemaSequence
                                                        {
                                                            Items =
                                                                {
                                                                    new XmlSchemaElement
                                                                    {
                                                                        Name = complexType.ListItemName,
                                                                        SchemaTypeName = complexType.QualifiedName,
                                                                        MinOccurs = 0,
                                                                        MaxOccurs = decimal.MaxValue
                                                                    }
                                                                }
                                                        }
                                         };
                        xmlSchema.Items.Add(xmlComplexType);
                    }
                }
                else if (type is SDataSchemaChoiceType)
                {
                    var xmlComplexType = new XmlSchemaComplexType();
                    type.Write(xmlComplexType);
                    xmlSchema.Items.Add(xmlComplexType);
                }
                else if (type is SDataSchemaSimpleType || type is SDataSchemaEnumType)
                {
                    var xmlType = new XmlSchemaSimpleType();
                    type.Write(xmlType);
                    xmlSchema.Items.Add(xmlType);
                }
                else
                {
                    throw new InvalidOperationException(string.Format("Unexpected type '{0}'", type.GetType()));
                }
            }
        }

        private void Compile()
        {
            var listTypes = ComplexTypes.Where(type => type.ListName != null)
                .Select(type => new {name = type.ListQualifiedName, type = (SDataSchemaType) type});
            var types = Types.Select(type => new {name = type.QualifiedName, type})
                .Concat(listTypes)
                .ToDictionary(type => type.name, type => type.type);
            Compile(types);
        }

        internal void Compile(IDictionary<XmlQualifiedName, SDataSchemaType> types)
        {
            foreach (var typeRef in Descendents().OfType<SDataSchemaTypeReference>())
            {
                SDataSchemaType type;

                if (types.TryGetValue(typeRef.QualifiedName, out type))
                {
                    typeRef.SchemaType = type;
                }
            }
        }
    }
}