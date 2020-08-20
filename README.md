# Objectify
A .NET library for cross program interactions without definitions

This library was created when I was attempting to write plugins for open source software that was constantly being updated.
I didn't want to have to rewrite the added code for every new version of the software, so I created a library that could easily
be added and used to handle the cross program interaction. The main usage was for plugins on the AsyncRAT project, but I found other
uses for it in my unfinished ML Builder, where I use it to import environments with a default structure but variable instructions.

Constructors
<code>
public Objectify(Assembly Container, String Class, Dictionary<String, object> parameters) {  }
</code>
This constructor is for creating a new instance of an object that exists in another assembly.

Container = The original executing assembly (this allows for bidirectional communication between both assemblies)
Class = The location of the structure that this object follows
parameters = the parameters that are passed to the remote constructor (string is the parameter name, and object is the value)

<code>
public Objectify(Assembly Container, String Class, object Instance) {  }
</code>
This constructor is for converting an existing instance of an object, to an Objectify instance.

Container = The original executing assembly (this allows for bidirectional communication between both assemblies)
Class = The location of the structure that this object follows
Instance = the original instance of the object

Example
-------------------------------------------------------------------------------------------------------------------

Plugin:

Dictionary<String, object> param = new Dictionary<String, object>();  
param.Add("Name", "Deadman");  
param.Add("Age", 99);  
param.Add("Face", (Bitmap)this.Properties.Resources.Image1;  
Objectify RemoteClass = new Objectify(SourceAssembly, "Plugin.RemoteStructure", param);  
object value = RemoteClass.CallFunction("MyFunction", new Dictionary<String, object>());  
Type VariableType = null;  
object value = RemoteClass.CallVariable("MyName", out VariableType);  

