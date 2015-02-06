using System;
using System.Xml.Serialization;
using System.IO;

namespace WorkEX
{
	//Класс определяющий какие настройки есть в программе
	public class PropsFields
	{
		//Путь до файла настроек
//		public String XMLFileName = Environment.CurrentDirectory +"\\settings.xml";
		public String XMLFileName = Path.Combine (Environment.GetFolderPath(Environment.SpecialFolder.Personal), "settings.xml");
//		public String XMLFileName = Environment.CurrentDirectory +"\\settings.xml";
//		string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

		//Чтобы добавить настройку в программу просто добавьте суда строку вида -
		//public ТИП ИМЯ_ПЕРЕМЕННОЙ = значение_переменной_по_умолчанию;
		public String UserIDValue = @"";
//		public DateTime DateValue = new DateTime(2011, 1, 1);
//		public Decimal DecimalValue = 555;
//		public Boolean BoolValue = true;
	}

	//Класс работы с настройками
	public class Props
	{
		public PropsFields Fields;

		public Props()
		{
			Fields = new PropsFields();
		}

		//Запист настроек в файл
		public void WriteXml()
		{
			XmlSerializer ser = new XmlSerializer(typeof(PropsFields));
			TextWriter writer = new StreamWriter(Fields.XMLFileName);
			ser.Serialize(writer, Fields);
			writer.Close();
		}
		//Чтение настроек из файла
		public void ReadXml()
		{
			if (File.Exists(Fields.XMLFileName)){
				XmlSerializer ser = new XmlSerializer(typeof(PropsFields));
				TextReader reader = new StreamReader(Fields.XMLFileName);
				Fields = ser.Deserialize(reader) as PropsFields;
				reader.Close();
			}
			else{//можно написать вывод какова то сообщения если файла не существует}
			}
		}
	}
}