﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CT.CASE.Generator.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CT.CASE.Generator.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to # imports: http://purl.org/co
        ///# imports: http://purl.org/spar/error
        ///# imports: https://ontology.caseontology.org/case/investigation/1.3.0
        ///# imports: https://ontology.caseontology.org/case/vocabulary/1.3.0
        ///# imports: https://ontology.unifiedcyberontology.org/co/1.3.0
        ///# imports: https://ontology.unifiedcyberontology.org/owl/1.3.0
        ///# imports: https://ontology.unifiedcyberontology.org/uco/action/1.3.0
        ///# imports: https://ontology.unifiedcyberontology.org/uco/analysis/1.3.0
        ///# imports: https://ontology.unifiedcyber [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string Case_1_3_0 {
            get {
                return ResourceManager.GetString("Case_1_3_0", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PREFIX owl: &lt;http://www.w3.org/2002/07/owl#&gt;
        ///PREFIX sh: &lt;http://www.w3.org/ns/shacl#&gt;
        ///PREFIX rdfs: &lt;http://www.w3.org/2000/01/rdf-schema#&gt;
        ///PREFIX rdf: &lt;http://www.w3.org/1999/02/22-rdf-syntax-ns#&gt;
        ///PREFIX observable: &lt;https://ontology.unifiedcyberontology.org/uco/observable/&gt;
        ///
        ///SELECT ?class ?subClassOf ?comment
        ///WHERE
        ///{
        ///	?class a owl:Class.
        ///	?shape a sh:NodeShape ;
        ///		sh:targetClass ?class .
        ///	OPTIONAL{?class rdfs:subClassOf ?subClassOf.}
        ///	OPTIONAL{?class rdfs:comment ?comment.}
        ///}
        ///.
        /// </summary>
        internal static string ClassQuery {
            get {
                return ResourceManager.GetString("ClassQuery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PREFIX owl: &lt;http://www.w3.org/2002/07/owl#&gt;
        ///PREFIX rdf: &lt;http://www.w3.org/1999/02/22-rdf-syntax-ns#&gt;
        ///PREFIX rdfs: &lt;http://www.w3.org/2000/01/rdf-schema#&gt; 
        ///
        ///SELECT ?enum_uri ?list_item ?alias
        ///WHERE
        ///{
        ///    ?enum_uri
        ///        a rdfs:Datatype;
        ///        owl:oneOf/rdf:rest*/rdf:first ?list_item .
        /// 	?alias
        ///    	a rdfs:Datatype ;
        ///        owl:equivalentClass ?enum_uri .
        ///}.
        /// </summary>
        internal static string EnumQuery {
            get {
                return ResourceManager.GetString("EnumQuery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PREFIX owl: &lt;http://www.w3.org/2002/07/owl#&gt;
        ///PREFIX rdfs: &lt;http://www.w3.org/2000/01/rdf-schema#&gt; 
        ///PREFIX sh: &lt;http://www.w3.org/ns/shacl#&gt;
        ///
        ///SELECT ?prop ?range ?comment
        ///WHERE {
        ///    ?prop
        ///        a owl:DatatypeProperty ;
        ///        rdfs:range ?range .
        ///    OPTIONAL {
        ///        ?prop rdfs:comment ?comment .
        ///    }
        ///    FILTER NOT EXISTS {
        ///        ?cls sh:property ?bnode .
        ///        ?bnode sh:path ?prop .
        ///    }
        ///}.
        /// </summary>
        internal static string GlobalPropertiesQuery {
            get {
                return ResourceManager.GetString("GlobalPropertiesQuery", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PREFIX owl: &lt;http://www.w3.org/2002/07/owl#&gt;
        ///PREFIX sh: &lt;http://www.w3.org/ns/shacl#&gt;
        ///PREFIX rdfs: &lt;http://www.w3.org/2000/01/rdf-schema#&gt;
        ///PREFIX rdf: &lt;http://www.w3.org/1999/02/22-rdf-syntax-ns#&gt;
        ///PREFIX observable: &lt;https://ontology.unifiedcyberontology.org/uco/observable/&gt;
        ///
        ///SELECT ?class ?subClassOf ?path_uri ?target ?min ?max
        ///WHERE
        ///{
        ///	?class a owl:Class.
        ///	?shape a sh:NodeShape ;
        ///		sh:targetClass ?class .
        ///	?shape sh:property ?bnode .
        ///	OPTIONAL{?class rdfs:subClassOf ?subClassOf.}
        ///	OPTIONAL{? [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string PropertiesQuery {
            get {
                return ResourceManager.GetString("PropertiesQuery", resourceCulture);
            }
        }
    }
}
