﻿//-----------------------------------------------------------------------
// <copyright file="DictionaryXmlReader.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOAuth.Messaging {
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Xml;
	using System.Xml.Linq;

	/// <summary>
	/// An XmlReader-looking object that actually reads from a dictionary.
	/// </summary>
	internal class DictionaryXmlReader {
		/// <summary>
		/// Creates an XmlReader that reads data out of a dictionary instead of XML.
		/// </summary>
		/// <param name="rootElement">The name of the root XML element.</param>
		/// <param name="fieldSorter">The field sorter so that the XmlReader generates xml elements in the order required by the <see cref="DataContractSerializer"/>.</param>
		/// <param name="fields">The dictionary to read data from.</param>
		/// <returns>The XmlReader that will read the data out of the given dictionary.</returns>
		internal static XmlReader Create(XName rootElement, IComparer<string> fieldSorter, IDictionary<string, string> fields) {
			if (rootElement == null) {
				throw new ArgumentNullException("rootElement");
			}
			if (fieldSorter == null) {
				throw new ArgumentNullException("fieldSorter");
			}
			if (fields == null) {
				throw new ArgumentNullException("fields");
			}

			return CreateRoundtripReader(rootElement, fieldSorter, fields);
		}

		/// <summary>
		/// Creates an <see cref="XmlReader"/> that will read values out of a dictionary.
		/// </summary>
		/// <param name="rootElement">The surrounding root XML element to generate.</param>
		/// <param name="fieldSorter">The field sorter so that the XmlReader generates xml elements in the order required by the <see cref="DataContractSerializer"/>.</param>
		/// <param name="fields">The dictionary to list values from.</param>
		/// <returns>The generated <see cref="XmlReader"/>.</returns>
		private static XmlReader CreateRoundtripReader(XName rootElement, IComparer<string> fieldSorter, IDictionary<string, string> fields) {
			Debug.Assert(rootElement != null, "rootElement == null");
			Debug.Assert(fieldSorter != null, "fieldSorter == null");
			Debug.Assert(fields != null, "fields == null");

			MemoryStream stream = new MemoryStream();
			XmlWriter writer = XmlWriter.Create(stream);
			SerializeDictionaryToXml(writer, rootElement, fieldSorter, fields);
			writer.Flush();
			stream.Seek(0, SeekOrigin.Begin);

			// For debugging purposes.
			StreamReader sr = new StreamReader(stream);
			Trace.WriteLine(sr.ReadToEnd());
			stream.Seek(0, SeekOrigin.Begin);

			return XmlReader.Create(stream);
		}

		/// <summary>
		/// Writes out the values in a dictionary as XML.
		/// </summary>
		/// <param name="writer">The <see cref="XmlWriter"/> to write out the XML to.</param>
		/// <param name="rootElement">The name of the root element to use to surround the dictionary values.</param>
		/// <param name="fieldSorter">The field sorter so that the XmlReader generates xml elements in the order required by the <see cref="DataContractSerializer"/>.</param>
		/// <param name="fields">The dictionary with values to serialize.</param>
		private static void SerializeDictionaryToXml(XmlWriter writer, XName rootElement, IComparer<string> fieldSorter, IDictionary<string, string> fields) {
			Debug.Assert(writer != null, "writer == null");
			Debug.Assert(rootElement != null, "rootElement == null");
			Debug.Assert(fields != null, "fields == null");

			writer.WriteStartElement(rootElement.LocalName, rootElement.NamespaceName);

			foreach (var pair in fields.OrderBy(pair => pair.Key, fieldSorter)) {
				writer.WriteStartElement(pair.Key, rootElement.NamespaceName);
				writer.WriteValue(pair.Value);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}
	}
}
