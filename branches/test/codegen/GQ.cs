
using System;
using System.IO;
using System.Collections;
using com.beastsoft.templates;

using System.Windows.Forms;


    public class RenderClass
    {
        private TemplateProject templateProject;

    #line 1

public class Util
{
	public static void MakeDirectory(string path)
	{
		try
		{
			if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
       	}
       	catch(Exception ex){}
	}
}

        
        public void Render(TemplateProject templateProject)
        {
            this.templateProject = templateProject;

#line 2

string clientPath = templateProject.directoryPath + @"client\"+ templateProject.namespaceBase.Replace(".", @"\")+@"\";
string serverPath = templateProject.directoryPath + templateProject.namespaceBase.Replace(".", @"\")+@"\";

StreamWriter writer = null;
/*

GENERACION CLASE DOMAIN CODEGEN

*/

Util.MakeDirectory(serverPath+"domain");
writer = new StreamWriter(serverPath + @"domain\codegen.cs");


		

#line 3

            writer.Write("using GQService.com.gq.dto;\r\nusing System;\r\n\r\nnamespace ");

#line 4

            writer.Write(templateProject.namespaceBase);

#line 5

            writer.Write(".domain.codegen\r\n{\r\n");

#line 6

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{

#line 7

            writer.Write("\r\n    public class _");

#line 8

            writer.Write(table.tableNameClassName);

#line 9

            writer.Write(" : IEntity\r\n    {\r\n");

#line 10
			
		foreach(Columns column in table.columns())
		{

#line 11

            writer.Write("\r\n        public virtual ");

#line 12

            writer.Write(column.CSharpType.FullName);

#line 13

            writer.Write(((column.isPrimary==true||column.isNullable) && (column.CSharpType.FullName!="System.String" && column.CSharpType.FullName!="System.Byte[]"))?"?":"");

#line 14

            writer.Write(" ");

#line 15

            writer.Write(column.getColumnNameUpper());

#line 16

            writer.Write(" { get; set; }\r\n\r\n");

#line 17

		}
#line 18

            writer.Write("\r\n    }\r\n");

#line 19

		
	}
}

#line 20

            writer.Write("\r\n}\r\n");

#line 21

writer.Flush();
writer.Close();
writer.Dispose();

/*

GENERACION CLASE DOMAIN

*/

Util.MakeDirectory(serverPath+"domain");

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{
		writer = new StreamWriter(serverPath + @"domain\" + table.tableNameClassName + ".cs");

#line 22

            writer.Write("\r\nusing ");

#line 23

            writer.Write(templateProject.namespaceBase);

#line 24

            writer.Write(".domain.codegen;\r\nusing System;\r\n\r\nnamespace ");

#line 25

            writer.Write(templateProject.namespaceBase);

#line 26

            writer.Write(".domain\r\n{\r\n    public class ");

#line 27

            writer.Write(table.tableNameClassName);

#line 28

            writer.Write(":_");

#line 29

            writer.Write(table.tableNameClassName);

#line 30

            writer.Write("\r\n    {\r\n    }\r\n}\r\n");

#line 31

		writer.Flush();
		writer.Close();
		writer.Dispose();
	}
}


/*

GENERACION CLASE JS DTO

*/

Util.MakeDirectory(serverPath+"JSDTO");
writer = new StreamWriter(serverPath + @"JSDTO\dto.js");

#line 32

            writer.Write("\r\nFunction.prototype.extends = function (parent) {\r\n    this.prototype = Object.create(parent.prototype);\r\n};\r\n");

#line 33

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{

#line 34

            writer.Write("\r\n\r\nvar _");

#line 35

            writer.Write(table.tableNameClassName);

#line 36

            writer.Write("Dto = function (value) {\r\n	\r\n	value = (value==null||value===undefined)?{}:value;\r\n	\r\n	if(isArray(value))\r\n    {\r\n        var result = value;\r\n\r\n        for (var i = 0; i < result.length; i++) {\r\n        	result[i].selfData = undefined;\r\n            result[i] = new ");

#line 37

            writer.Write(table.tableNameClassName);

#line 38

            writer.Write("Dto(result[i]);\r\n        }\r\n\r\n        return result;\r\n    }\r\n    \r\n    value.selfData = undefined;\r\n    \r\n    var self = this;\r\n\r\n	self.selfData = value;\r\n	\r\n    self.className = \"");

#line 39

            writer.Write(templateProject.namespaceBase);

#line 40

            writer.Write(".dto.");

#line 41

            writer.Write(table.tableNameClassName);

#line 42

            writer.Write("Dto\";\r\n");

#line 43
			
		foreach(Columns column in table.columns())
		{

#line 44

            writer.Write("\r\n    self.");

#line 45

            writer.Write(column.getColumnNameUpper());

#line 46

            writer.Write("=value.");

#line 47

            writer.Write(column.getColumnNameUpper());

#line 48

            writer.Write("===undefined?");

#line 49

            writer.Write((((column.isPrimary==true||column.isNullable) && column.CSharpType.FullName!="System.String")?" null ":(column.CSharpType.FullName=="System.String"?" \"\" ":(column.CSharpType.FullName=="System.DateTime"?" new Date() ":" 0 "))) );

#line 50

            writer.Write(":value.");

#line 51

            writer.Write(column.getColumnNameUpper());

#line 52

            writer.Write(";\r\n");

#line 53
		}
#line 54

            writer.Write("\r\n};\r\n");

#line 55

		
	}
}
writer.Flush();
writer.Close();
writer.Dispose();


/*

GENERACION CLASE JS DTO

*/

Util.MakeDirectory(serverPath+"JSDTO");

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{
		writer = new StreamWriter(serverPath + @"JSDTO\" + table.tableNameClassName + "Dto.js");

#line 56

            writer.Write("\r\nvar ");

#line 57

            writer.Write(table.tableNameClassName);

#line 58

            writer.Write("Dto = function(value) {\r\n    var self = this;\r\n\r\n    var data = _");

#line 59

            writer.Write(table.tableNameClassName);

#line 60

            writer.Write("Dto.apply(self, arguments);\r\n    if (isArray(data)) {\r\n        return data;\r\n    }\r\n	value = (value==null||value===undefined)?{}:value;\r\n}\r\n\r\n");

#line 61

            writer.Write(table.tableNameClassName);

#line 62

            writer.Write("Dto.extends(_");

#line 63

            writer.Write(table.tableNameClassName);

#line 64

            writer.Write("Dto);\r\n");

#line 65

		writer.Flush();
		writer.Close();
		writer.Dispose();
	}
}


/*

GENERACION CLASE DTO CODEGEN

*/

Util.MakeDirectory(serverPath+"dto");

writer = new StreamWriter(serverPath + @"dto\codegen.cs");


#line 66

            writer.Write("\r\nusing ");

#line 67

            writer.Write(templateProject.namespaceBase);

#line 68

            writer.Write(".domain;\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing GQService.com.gq.dto;\r\n\r\nnamespace ");

#line 69

            writer.Write(templateProject.namespaceBase);

#line 70

            writer.Write(".dto.codegen\r\n{\r\n");

#line 71

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{

#line 72

            writer.Write("\r\n\r\n    public class _");

#line 73

            writer.Write(table.tableNameClassName);

#line 74

            writer.Write("Dto : Dto<");

#line 75

            writer.Write(table.tableNameClassName);

#line 76

            writer.Write(",");

#line 77

            writer.Write(table.tableNameClassName);

#line 78

            writer.Write("Dto>\r\n    {\r\n    	public _");

#line 79

            writer.Write(table.tableNameClassName);

#line 80

            writer.Write("Dto() : base()\r\n    	{\r\n    	}\r\n    	\r\n    	public _");

#line 81

            writer.Write(table.tableNameClassName);

#line 82

            writer.Write("Dto( ");

#line 83

            writer.Write(table.tableNameClassName);

#line 84

            writer.Write(" value) : base(value)\r\n    	{\r\n    	}\r\n");

#line 85
			
		foreach(Columns column in table.columns())
		{

#line 86

            writer.Write("\r\n        public virtual ");

#line 87

            writer.Write(column.CSharpType.FullName);

#line 88

            writer.Write(((column.isPrimary==true||column.isNullable) && (column.CSharpType.FullName!="System.String" && column.CSharpType.FullName!="System.Byte[]"))?"?":"");

#line 89

            writer.Write(" ");

#line 90

            writer.Write(column.getColumnNameUpper());

#line 91

            writer.Write(" { get; set; }\r\n\r\n");

#line 92

		}
#line 93

            writer.Write("\r\n    }\r\n");

#line 94

	}
}

#line 95

            writer.Write("\r\n}\r\n");

#line 96

writer.Flush();
writer.Close();
writer.Dispose();

/*

GENERACION CLASE DTO


*/

Util.MakeDirectory(serverPath+"dto");

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{
		writer = new StreamWriter(serverPath + @"dto\" + table.tableNameClassName + "Dto.cs");

#line 97

            writer.Write("\r\nusing ");

#line 98

            writer.Write(templateProject.namespaceBase);

#line 99

            writer.Write(".domain;\r\nusing ");

#line 100

            writer.Write(templateProject.namespaceBase);

#line 101

            writer.Write(".dto.codegen;\r\nusing System;\r\nusing System.Collections.Generic;\r\n\r\nnamespace ");

#line 102

            writer.Write(templateProject.namespaceBase);

#line 103

            writer.Write(".dto\r\n{\r\n    public class ");

#line 104

            writer.Write(table.tableNameClassName);

#line 105

            writer.Write("Dto : _");

#line 106

            writer.Write(table.tableNameClassName);

#line 107

            writer.Write("Dto\r\n    {\r\n        public ");

#line 108

            writer.Write(table.tableNameClassName);

#line 109

            writer.Write("Dto():base()\r\n        {\r\n        }\r\n       \r\n        public ");

#line 110

            writer.Write(table.tableNameClassName);

#line 111

            writer.Write("Dto(");

#line 112

            writer.Write(table.tableNameClassName);

#line 113

            writer.Write(" value):base(value)\r\n        {\r\n        }\r\n    }\r\n}\r\n");

#line 114

		writer.Flush();
		writer.Close();
		writer.Dispose();
	}
}

/*

GENERACION CLASE XAMARIN


*/
/*

GENERACION CLASE XAMARIN DTO CODEGEN

*/

Util.MakeDirectory(serverPath+"Xamarin_dto");

writer = new StreamWriter(serverPath + @"Xamarin_dto\codegen.cs");


#line 115

            writer.Write("\r\nusing System;\r\nusing System.Collections.Generic;\r\n\r\nnamespace ");

#line 116

            writer.Write(templateProject.namespaceBase);

#line 117

            writer.Write(".dto.codegen\r\n{\r\n");

#line 118

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{

#line 119

            writer.Write("\r\n\r\n    public class _");

#line 120

            writer.Write(table.tableNameClassName);

#line 121

            writer.Write("Dto\r\n    {\r\n");

#line 122
			
		foreach(Columns column in table.columns())
		{

#line 123

            writer.Write("\r\n        public virtual ");

#line 124

            writer.Write(column.CSharpType.FullName);

#line 125

            writer.Write(((column.isPrimary==true||column.isNullable) && (column.CSharpType.FullName!="System.String" && column.CSharpType.FullName!="System.Byte[]"))?"?":"");

#line 126

            writer.Write(" ");

#line 127

            writer.Write(column.getColumnNameUpper());

#line 128

            writer.Write(" { get; set; }\r\n\r\n");

#line 129

		}
#line 130

            writer.Write("\r\n    }\r\n");

#line 131

	}
}

#line 132

            writer.Write("\r\n}\r\n");

#line 133

writer.Flush();
writer.Close();
writer.Dispose();

/*

GENERACION CLASE XAMARIN DTO


*/

Util.MakeDirectory(serverPath+"Xamarin_dto");

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{
		writer = new StreamWriter(serverPath + @"Xamarin_dto\" + table.tableNameClassName + "Dto.cs");

#line 134

            writer.Write("\r\nusing ");

#line 135

            writer.Write(templateProject.namespaceBase);

#line 136

            writer.Write(".dto.codegen;\r\nusing System;\r\nusing System.Collections.Generic;\r\n\r\nnamespace ");

#line 137

            writer.Write(templateProject.namespaceBase);

#line 138

            writer.Write(".dto\r\n{\r\n    public class ");

#line 139

            writer.Write(table.tableNameClassName);

#line 140

            writer.Write("Dto : _");

#line 141

            writer.Write(table.tableNameClassName);

#line 142

            writer.Write("Dto\r\n    {\r\n\r\n    }\r\n}\r\n");

#line 143

		writer.Flush();
		writer.Close();
		writer.Dispose();
	}
}


/*

GENERACION CLASE MAPPER CODEGEN


*/

Util.MakeDirectory(serverPath+"mapping");
writer = new StreamWriter(serverPath + @"mapping\codegen.cs");
	

#line 144

            writer.Write("\r\nusing FluentNHibernate.Mapping;\r\nusing ");

#line 145

            writer.Write(templateProject.namespaceBase);

#line 146

            writer.Write(".domain;\r\n\r\nnamespace ");

#line 147

            writer.Write(templateProject.namespaceBase);

#line 148

            writer.Write(".mapping.codegen\r\n{\r\n");

#line 149

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{

#line 150

            writer.Write("\r\n    public class _Map");

#line 151

            writer.Write(table.tableNameClassName);

#line 152

            writer.Write(" : ClassMap<");

#line 153

            writer.Write(table.tableNameClassName);

#line 154

            writer.Write(">\r\n    {\r\n        public _Map");

#line 155

            writer.Write(table.tableNameClassName);

#line 156

            writer.Write("()\r\n        {\r\n        	Table(\"");

#line 157

            writer.Write(table.tableNameClassName.ToLower());

#line 158

            writer.Write("\");\r\n        	\r\n");

#line 159
			
		foreach(Columns column in table.columns())
		{
			if(column.isPrimary==true)
			{
				if(column.extra=="auto_increment"|| column.AutoIncrement){
		
#line 160

            writer.Write("			Id(c => c.");

#line 161

            writer.Write(column.getColumnNameUpper());

#line 162

            writer.Write(").GeneratedBy.Identity();");

#line 163

				}else{
		
#line 164

            writer.Write("			Id(c => c.");

#line 165

            writer.Write(column.getColumnNameUpper());

#line 166

            writer.Write(").GeneratedBy.Assigned();");

#line 167
				
			}}
			else
			{
#line 168

            writer.Write("\r\n			Map(c => c.");

#line 169

            writer.Write(column.getColumnNameUpper());

#line 170

            writer.Write(")");

#line 171

            writer.Write((column.CSharpType.FullName!="System.String"?"":".Length(" + (column.maximunLength==-1?int.MaxValue:column.maximunLength) +")"));

#line 172

            writer.Write(";\r\n");

#line 173
			}
		}
#line 174

            writer.Write("\r\n		}\r\n    }\r\n");

#line 175


	}
}

#line 176

            writer.Write("\r\n}\r\n");

#line 177


writer.Flush();
writer.Close();
writer.Dispose();


/*

GENERACION CLASE MAPPER


*/

Util.MakeDirectory(serverPath+"mapping");

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{
		writer = new StreamWriter(serverPath + @"mapping\Map" + table.tableNameClassName + ".cs");

#line 178

            writer.Write("\r\nusing FluentNHibernate.Mapping;\r\nusing ");

#line 179

            writer.Write(templateProject.namespaceBase);

#line 180

            writer.Write(".domain;\r\nusing ");

#line 181

            writer.Write(templateProject.namespaceBase);

#line 182

            writer.Write(".mapping.codegen;\r\n\r\nnamespace ");

#line 183

            writer.Write(templateProject.namespaceBase);

#line 184

            writer.Write(".mapping\r\n{\r\n    public class Map");

#line 185

            writer.Write(table.tableNameClassName);

#line 186

            writer.Write(" : _Map");

#line 187

            writer.Write(table.tableNameClassName);

#line 188

            writer.Write("\r\n    {\r\n        public Map");

#line 189

            writer.Write(table.tableNameClassName);

#line 190

            writer.Write("():base()\r\n        {\r\n        }\r\n    }\r\n}\r\n");

#line 191

		writer.Flush();
		writer.Close();
		writer.Dispose();
	}
}


/*

GENERACION CLASE MAPPER CODEGEN


*/

Util.MakeDirectory(serverPath+"mapping");
writer = new StreamWriter(serverPath + @"mapping\sessionMAP.txt");
	
foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{

#line 192

            writer.Write("Type.GetType(\"");

#line 193

            writer.Write(templateProject.namespaceBase);

#line 194

            writer.Write(".mapping.Map");

#line 195

            writer.Write(table.tableNameClassName);

#line 196

            writer.Write(", AlladioDataService\"),\r\n");

#line 197


	}
}


writer.Flush();
writer.Close();
writer.Dispose();




/*

GENERACION CLASE SERVICE


*/

Util.MakeDirectory(serverPath+"service");

writer = new StreamWriter(serverPath + @"service\codegen.cs");

#line 198

            writer.Write("\r\nusing ");

#line 199

            writer.Write(templateProject.namespaceBase);

#line 200

            writer.Write(".domain;\r\nusing GQService.com.gq.service;\r\nusing NHibernate;\r\n\r\nnamespace ");

#line 201

            writer.Write(templateProject.namespaceBase);

#line 202

            writer.Write(".service.codegen\r\n{\r\n\r\n");

#line 203

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{
	
		Columns primaryColumn=null;
		bool hasBorrar=false;
		
		foreach(Columns column in table.columns())
		{
			if(column.isPrimary==true)
			{
				primaryColumn = column;
			}
			if(column.getColumnNameUpper().Equals("Borrado"))
			{
				hasBorrar=true;
			}
		}

#line 204

            writer.Write("\r\n	public class _Serv");

#line 205

            writer.Write(table.tableNameClassName);

#line 206

            writer.Write(" : GenericService<");

#line 207

            writer.Write(table.tableNameClassName);

#line 208

            writer.Write(">\r\n    {\r\n    	#region Constructores\r\n\r\n        public _Serv");

#line 209

            writer.Write(table.tableNameClassName);

#line 210

            writer.Write("(ISession session): base(session){}\r\n        public _Serv");

#line 211

            writer.Write(table.tableNameClassName);

#line 212

            writer.Write("(IStatelessSession statelessSession): base(statelessSession){}\r\n        public _Serv");

#line 213

            writer.Write(table.tableNameClassName);

#line 214

            writer.Write("(ISession session, IStatelessSession statelessSession): base(session,statelessSession){}\r\n\r\n        #endregion\r\n             \r\n");

#line 215

		if(hasBorrar)
		{

#line 216

            writer.Write("   \r\n        public override bool Borrar(");

#line 217

            writer.Write(table.tableNameClassName);

#line 218

            writer.Write(" pObj)\r\n        {\r\n            pObj.Borrado = \"1\";\r\n            return base.Actualizar (pObj) != null;\r\n        }\r\n");

#line 219
	
		}

#line 220

            writer.Write("\r\n    }\r\n");

#line 221

	}
}

#line 222

            writer.Write("\r\n}\r\n");

#line 223

writer.Flush();
writer.Close();
writer.Dispose();

foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{
		writer = new StreamWriter(serverPath + @"service\Serv" + table.tableNameClassName + ".cs");
		Columns primaryColumn=null;
		foreach(Columns column in table.columns())
		{
			if(column.isPrimary==true)
			{
				primaryColumn = column;
				break;
			}
		}


#line 224

            writer.Write("\r\nusing ");

#line 225

            writer.Write(templateProject.namespaceBase);

#line 226

            writer.Write(".service.codegen;\r\nusing NHibernate;\r\n\r\nnamespace ");

#line 227

            writer.Write(templateProject.namespaceBase);

#line 228

            writer.Write(".service\r\n{\r\n    public class Serv");

#line 229

            writer.Write(table.tableNameClassName);

#line 230

            writer.Write(" : _Serv");

#line 231

            writer.Write(table.tableNameClassName);

#line 232

            writer.Write("\r\n    {\r\n    	#region Constructores\r\n\r\n        public Serv");

#line 233

            writer.Write(table.tableNameClassName);

#line 234

            writer.Write("(ISession session): base(session){}\r\n        public Serv");

#line 235

            writer.Write(table.tableNameClassName);

#line 236

            writer.Write("(IStatelessSession statelessSession): base(statelessSession){}\r\n        public Serv");

#line 237

            writer.Write(table.tableNameClassName);

#line 238

            writer.Write("(ISession session, IStatelessSession statelessSession): base(session,statelessSession){}\r\n\r\n        #endregion\r\n    }\r\n}\r\n");

#line 239

		writer.Flush();
		writer.Close();
		writer.Dispose();
	}
}

/*

GENERACION CLASE ANDROID


*/

Util.MakeDirectory(serverPath +  @"android");
Util.MakeDirectory(serverPath +  @"android\codegen");


foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{
		writer = new StreamWriter(serverPath + @"android\codegen\_" + table.tableNameClassName + ".java");

#line 240

            writer.Write("\r\npackage ");

#line 241

            writer.Write(templateProject.namespaceBase);

#line 242

            writer.Write(".next.data.codegen;\r\n\r\nimport com.alladio.next.data.IDto;\r\nimport com.alladio.next.utils.AlladioUtils;\r\nimport java.util.Date;\r\nimport android.database.Cursor;\r\nimport java.util.LinkedHashMap;\r\nimport java.math.BigDecimal;\r\n\r\npublic class _");

#line 243

            writer.Write(table.tableNameClassName);

#line 244

            writer.Write(" implements IDto {\r\n\r\n	public String getClassName(){return \"");

#line 245

            writer.Write(templateProject.namespaceBase);

#line 246

            writer.Write(".dto.");

#line 247

            writer.Write(table.tableNameClassName);

#line 248

            writer.Write("Dto\";}\r\n");

#line 249

		foreach(Columns column in table.columns())
		{
			
			if(column.getJavaType().Equals("ulong")==true){

#line 250

            writer.Write("	\r\n	private long ");

#line 251

            writer.Write(column.getColumnNameUpper());

#line 252

            writer.Write(";\r\n	public long get");

#line 253

            writer.Write(column.getColumnNameUpper());

#line 254

            writer.Write("(){return ");

#line 255

            writer.Write(column.getColumnNameUpper());

#line 256

            writer.Write(";}\r\n	public void set");

#line 257

            writer.Write(column.getColumnNameUpper());

#line 258

            writer.Write("(long ");

#line 259

            writer.Write(column.getColumnNameUpper());

#line 260

            writer.Write("){this.");

#line 261

            writer.Write(column.getColumnNameUpper());

#line 262

            writer.Write(" = ");

#line 263

            writer.Write(column.getColumnNameUpper());

#line 264

            writer.Write(";}\r\n");

#line 265
			}
			else if(column.getJavaType().Equals("byte")==true){

#line 266

            writer.Write("	\r\n	private int ");

#line 267

            writer.Write(column.getColumnNameUpper());

#line 268

            writer.Write(";\r\n	public int get");

#line 269

            writer.Write(column.getColumnNameUpper());

#line 270

            writer.Write("(){return ");

#line 271

            writer.Write(column.getColumnNameUpper());

#line 272

            writer.Write(";}\r\n	public void set");

#line 273

            writer.Write(column.getColumnNameUpper());

#line 274

            writer.Write("(int ");

#line 275

            writer.Write(column.getColumnNameUpper());

#line 276

            writer.Write("){this.");

#line 277

            writer.Write(column.getColumnNameUpper());

#line 278

            writer.Write(" = ");

#line 279

            writer.Write(column.getColumnNameUpper());

#line 280

            writer.Write(";}\r\n");

#line 281
			}
			else{

#line 282

            writer.Write("	\r\n	private ");

#line 283

            writer.Write(column.getJavaType());

#line 284

            writer.Write(" ");

#line 285

            writer.Write(column.getColumnNameUpper());

#line 286

            writer.Write(";\r\n	public ");

#line 287

            writer.Write(column.getJavaType());

#line 288

            writer.Write(" get");

#line 289

            writer.Write(column.getColumnNameUpper());

#line 290

            writer.Write("(){return ");

#line 291

            writer.Write(column.getColumnNameUpper());

#line 292

            writer.Write(";}\r\n	public void set");

#line 293

            writer.Write(column.getColumnNameUpper());

#line 294

            writer.Write("(");

#line 295

            writer.Write(column.getJavaType());

#line 296

            writer.Write(" ");

#line 297

            writer.Write(column.getColumnNameUpper());

#line 298

            writer.Write("){this.");

#line 299

            writer.Write(column.getColumnNameUpper());

#line 300

            writer.Write(" = ");

#line 301

            writer.Write(column.getColumnNameUpper());

#line 302

            writer.Write(";}\r\n");

#line 303
			}
		}

#line 304

            writer.Write("\r\n	public void setToCursor(Cursor cursor){\r\n");

#line 305

		foreach(Columns column in table.columns())
		{
			if(column.getJavaType().Equals("ulong")==true){

#line 306

            writer.Write("	\r\n		");

#line 307

            writer.Write(column.getColumnNameUpper());

#line 308

            writer.Write(" = cursor.getLong(cursor.getColumnIndex(\"");

#line 309

            writer.Write(column.getColumnNameUpper());

#line 310

            writer.Write("\"));\r\n");

#line 311

			}
			else if(column.getJavaType().Equals("byte")==true){

#line 312

            writer.Write("	\r\n		");

#line 313

            writer.Write(column.getColumnNameUpper());

#line 314

            writer.Write(" = cursor.getInt(cursor.getColumnIndex(\"");

#line 315

            writer.Write(column.getColumnNameUpper());

#line 316

            writer.Write("\"));\r\n");

#line 317

			}
			else if(column.getJavaType().Equals("BigDecimal")==true){

#line 318

            writer.Write("	\r\n		");

#line 319

            writer.Write(column.getColumnNameUpper());

#line 320

            writer.Write(" = BigDecimal.valueOf(cursor.getDouble(cursor.getColumnIndex(\"");

#line 321

            writer.Write(column.getColumnNameUpper());

#line 322

            writer.Write("\")));\r\n");

#line 323

			}
			else if(column.getJavaType().Equals("Date")==true){

#line 324

            writer.Write("	\r\n		");

#line 325

            writer.Write(column.getColumnNameUpper());

#line 326

            writer.Write(" = new Date(cursor.getLong(cursor.getColumnIndex(\"");

#line 327

            writer.Write(column.getColumnNameUpper());

#line 328

            writer.Write("\")));\r\n");

#line 329

			}else if(column.getJavaType().Equals("ulong")==true){

#line 330

            writer.Write("	\r\n		");

#line 331

            writer.Write(column.getColumnNameUpper());

#line 332

            writer.Write(" = new Date(cursor.getLong(cursor.getColumnIndex(\"");

#line 333

            writer.Write(column.getColumnNameUpper());

#line 334

            writer.Write("\")));\r\n");

#line 335

			}
			else{

#line 336

            writer.Write("	\r\n		");

#line 337

            writer.Write(column.getColumnNameUpper());

#line 338

            writer.Write(" = cursor.get");

#line 339

            writer.Write( column.getJavaType().Substring(0,1).ToUpper() + column.getJavaType().Substring(1,column.getJavaType().Length-1) );

#line 340

            writer.Write("(cursor.getColumnIndex(\"");

#line 341

            writer.Write(column.getColumnNameUpper());

#line 342

            writer.Write("\"));\r\n");

#line 343
			
			}
		}

#line 344

            writer.Write("		\r\n	}\r\n	\r\n	public void setToHashmap(LinkedHashMap<String, Object> data)\r\n	{\r\n");

#line 345

		foreach(Columns column in table.columns())
		{
			if(column.getJavaType().Equals("ulong")==true){

#line 346

            writer.Write("	\r\n		");

#line 347

            writer.Write(column.getColumnNameUpper());

#line 348

            writer.Write(" = Long.valueOf(AlladioUtils.getValue(data.get(\"");

#line 349

            writer.Write(column.getColumnNameUpper());

#line 350

            writer.Write("\"), \"0\"));\r\n");

#line 351

			}
			else if(column.getJavaType().Equals("Date")==true){

#line 352

            writer.Write("	\r\n		");

#line 353

            writer.Write(column.getColumnNameUpper());

#line 354

            writer.Write(" = new Date(Long.parseLong(data.get(\"");

#line 355

            writer.Write(column.getColumnNameUpper());

#line 356

            writer.Write("\").toString().replaceAll(\"/Date\",\"\").replaceAll(\"/\",\"\").replace('(', ' ').replace(')', ' ').replaceAll(\" \",\"\")));\r\n");

#line 357

			}
			else if(column.getJavaType().Equals("String")==true){

#line 358

            writer.Write("	\r\n		");

#line 359

            writer.Write(column.getColumnNameUpper());

#line 360

            writer.Write(" = (");

#line 361

            writer.Write(column.getJavaType());

#line 362

            writer.Write(") data.get(\"");

#line 363

            writer.Write(column.getColumnNameUpper());

#line 364

            writer.Write("\");\r\n");

#line 365
			
			}
			else if(column.getJavaType().Equals("int")==true || 
				column.getJavaType().Equals("byte")==true){

#line 366

            writer.Write("	\r\n		");

#line 367

            writer.Write(column.getColumnNameUpper());

#line 368

            writer.Write(" = Integer.valueOf(AlladioUtils.getValue(data.get(\"");

#line 369

            writer.Write(column.getColumnNameUpper());

#line 370

            writer.Write("\"), \"0\"));\r\n");

#line 371
			
			}
			else if(column.getJavaType().Equals("BigDecimal")==true){

#line 372

            writer.Write("	\r\n		");

#line 373

            writer.Write(column.getColumnNameUpper());

#line 374

            writer.Write(" = BigDecimal.valueOf(Double.valueOf(AlladioUtils.getValue(data.get(\"");

#line 375

            writer.Write(column.getColumnNameUpper());

#line 376

            writer.Write("\"), \"0\")));\r\n");

#line 377
			
			}
			else
			{

#line 378

            writer.Write("	\r\n		");

#line 379

            writer.Write(column.getColumnNameUpper());

#line 380

            writer.Write(" = ");

#line 381

            writer.Write( column.getJavaType().Substring(0,1).ToUpper() + column.getJavaType().Substring(1,column.getJavaType().Length-1) );

#line 382

            writer.Write(".valueOf(AlladioUtils.getValue(data.get(\"");

#line 383

            writer.Write(column.getColumnNameUpper());

#line 384

            writer.Write("\"), \"0\"));\r\n");

#line 385
				
			}
		}

#line 386

            writer.Write("		\r\n	}		\r\n}\r\n");

#line 387
	
	writer.Flush();
	writer.Close();
	writer.Dispose();
	}
}


foreach(Tables table in templateProject.tables)
{
	if(table.selected)
	{
		writer = new StreamWriter(serverPath + @"android\" + table.tableNameClassName + ".java");

#line 388

            writer.Write("\r\npackage ");

#line 389

            writer.Write(templateProject.namespaceBase);

#line 390

            writer.Write(".next.data;\r\n\r\nimport  ");

#line 391

            writer.Write(templateProject.namespaceBase);

#line 392

            writer.Write(".next.data.codegen.*;\r\n\r\npublic class ");

#line 393

            writer.Write(table.tableNameClassName);

#line 394

            writer.Write(" extends _");

#line 395

            writer.Write(table.tableNameClassName);

#line 396

            writer.Write(" {\r\n\r\n}\r\n");

#line 397
	
	writer.Flush();
	writer.Close();
	writer.Dispose();
	}
}


System.Diagnostics.Process.Start(serverPath);


        }
    }