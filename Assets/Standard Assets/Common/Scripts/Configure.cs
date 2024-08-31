//! @file Configure.cs


using System.Collections;


//! @class Configure
//! @brief ������
public class Configure
{
	//! @enum ValueType
	//! @brief ����ֵ����
	private enum ValueType
	{
		Single,		//!< ��ֵ
		Array,		//!< һά����
		Array2		//!< ��ά����
	}

	//! @class Value
	//! ������
	private class Value
	{
		public string key;
		public ValueType type;
		public string val_single;
		public ArrayList val_array = new ArrayList();
		public ArrayList val_array2 = new ArrayList();
		public string comment1;
		public string comment2;
	}

	//! @class Section
	//! ���ö�
	private class Section
	{
		public string section;
		public string comment1;
		public string comment2;
		public ArrayList values = new ArrayList();
	}

	//! ��������
	public ArrayList m_configure = new ArrayList();


	public Configure()
	{
	}

	//! ��������
	public bool Load(string data)
	{
		ArrayList lines = new ArrayList();
		ArrayList last_comments = new ArrayList();
		ArrayList line_comments = new ArrayList();

		int data_len = data.Length;
		int index = 0;

		string l = "";
		string last_comment = "";

		// ��������
		while (true)
		{
			// get a line
			int index1, index2;
			for (index1 = index; index1 < data_len; index1++)
			{
				if ((data[index1] != '\r') && (data[index1] != '\n')) {
					break;
				}
			}
			if (index1 >= data_len)
			{
				break;
			}
			for (index2 = index1 + 1; index2 < data_len; index2++)
			{
				if ((data[index2] == '\r') || (data[index2] == '\n')) {
					break;
				}
			}

			index = index2 + 1;

			l = data.Substring(index1, index2 - index1);

			string line_comment = "";

			// ȡ��ע��
			int comment_pos = l.IndexOf('#');
			if (comment_pos != -1)
			{
				line_comment = l.Substring(comment_pos + 1);

				l = l.Substring(0, comment_pos);
			}

			// trim " \t"
			l = l.Trim();

			// ���Կ��к���������
			if (l.Length < 1) {
				// ����Ϊһ����ע����
				if (line_comment.Length >= 1)
				{
					last_comment = line_comment;
				}
				continue;
			}

			//
			string line = l;

			lines.Add(line);
			last_comments.Add(last_comment.Trim());
			line_comments.Add(line_comment.Trim());

			last_comment = "";
		}

		// ��������
		string section = "";
		for (int i = 0; i < lines.Count; i++)
		{
			string line = lines[i] as string;
	
			// ��¼section
			if ((line.Length > 2) && (line[0] == '[') && (line[line.Length - 1] == ']'))
			{
				section = line.Substring(1, line.Length - 2);
				AddSection(section, last_comments[i] as string, line_comments[i] as string);
				continue;
			}

			// ȡ��key-value
			int pos = line.IndexOf('=');
			if (pos == -1)
			{
				continue;
			}

			string key = line.Substring(0, pos);
			string value = line.Substring(pos + 1);

			key = key.Trim();
			value = value.Trim();

			if (value == "(")
			{
				ArrayList val = new ArrayList();
				int x = i;
				for (i = i + 1; i < lines.Count; i++)
				{
					line = lines[i] as string;
					if (line[0] == ')')
					{
						break;
					}
	
					if ((line.Length > 2) && (line[0] == '(') && (line[line.Length - 1] == ')'))
					{
						string v = line.Substring(1, line.Length - 2);
						val.Add(v);
					}
					else
					{
						return false;
					}
				}

				AddValueArray2(section, key, val, last_comments[x] as string, line_comments[x] as string);
			}
			else if ((value.Length > 2) && (value[0] == '(') && (value[value.Length - 1] == ')'))
			{
				string v = value.Substring(1, value.Length - 2);
				AddValueArray(section, key, v, last_comments[i] as string, line_comments[i] as string);
			}
			else
			{
				AddValueSingle(section, key, value, last_comments[i] as string, line_comments[i] as string);
			}
		}

		return true;
	}

	//! ��������
	public string Save()
	{
		string data = "";
		string buf;

		for (int i = 0; i < m_configure.Count; i++)
		{
			Section sec = m_configure[i] as Section;

			if (sec.comment1.Length >= 1)
			{
				buf = string.Format("#{0}\n", sec.comment1);
				data += buf;
			}

			buf = string.Format("[{0}]", sec.section);
			data += buf;
			if (sec.comment2.Length > 1)
			{
				buf = string.Format("\t#{0}", sec.comment2);
				data += buf;
			}
			data += "\n";

			for (int j = 0; j < sec.values.Count; j++)
			{
				Value val = sec.values[j] as Value;

				if (val.comment1.Length >= 1)
				{
					buf = string.Format("#{0}\n", val.comment1);
					data += buf;
				}

				if (val.type == ValueType.Single)
				{
					buf = string.Format("{0} = {1}", val.key, val.val_single);
					data += buf;
					if (val.comment2.Length > 1)
					{
						buf = string.Format("\t#{0}", val.comment2);
						data += buf;
					}
					data += "\n";
				}
				else if (val.type == ValueType.Array)
				{
					buf = string.Format("{0} = (", val.key);
					data += buf;

					for (int m = 0; m < val.val_array.Count; m++)
					{
						if (m != (val.val_array.Count - 1)) {
							data += val.val_array[m];
							data += " ";
						}
						else {
							data += val.val_array[m];
						}
					}
					data += ")";
					if (val.comment2.Length > 1) {
						buf = string.Format("\t#{0}", val.comment2);
						data += buf;
					}
					data +="\n";
				}
				else if (val.type == ValueType.Array2)
				{
					buf = string.Format("{0} = (", val.key);
					data += buf;
					if (val.comment2.Length > 1) {
						buf = string.Format("\t#{0}", val.comment2);
						data += buf;
					}
					data += "\n";

					for (int m = 0; m < val.val_array2.Count; m++)
					{
						data += "\t(";
						
						ArrayList temp = val.val_array2[m] as ArrayList;
	
						for (int n = 0; n < temp.Count; n++)
						{
							if (n != (temp.Count - 1)) {
								data += temp[n];
								data += " ";
							}
							else {
								data += temp[n];
							}
						}

						data += ")\n";
					}

					data += ")\n";
				}
			}

			data += "\n\n";
		}

		return data;
	}

	//! ȡ��ֵ����
	//! @param section ������
	//! @param key ������
	//! @return not null:�ɹ�, null:ʧ��
	public string GetSingle(string section, string key)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key ������
			return null;
		}

		if (v.type != ValueType.Single)
		{
			// ֵ���ʹ���
			return null;
		}

		return v.val_single;
	}

	//! ȡһά��������Ԫ�ظ���
	//! @param section ������
	//! @param key ������
	//! @return >=:�ɹ�, -1:ʧ��
	public int CountArray(string section, string key)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key ������
			return -1;
		}

		if (v.type != ValueType.Array)
		{
			// ֵ���ʹ���
			return -1;
		}

		return v.val_array.Count;
	}

	//! ȡһά��������
	//! @param section ������
	//! @param key ������
	//! @param i Ԫ���±�
	//! @return not null:�ɹ�, null:ʧ��
	public string GetArray(string section, string key, int i)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key ������
			return null;
		}

		if (v.type != ValueType.Array)
		{
			// ֵ���ʹ���
			return null;
		}

		int count = v.val_array.Count;
		if ((i < 0) || (i >= count))
		{
			// �±귶Χ����
			return null;
		}

		return v.val_array[i] as string;
	}

	//! ȡ��ά��������Ԫ�ص�һά����
	//! @param section ������
	//! @param key ������
	//! @return >=:�ɹ�, -1:ʧ��
	public int CountArray2(string section, string key)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key ������
			return -1;
		}

		if (v.type != ValueType.Array2)
		{
			// ֵ���ʹ���
			return -1;
		}

		return v.val_array2.Count;
	}

	//! ȡ��ά��������Ԫ�صڶ�ά����
	//! @param section ������
	//! @param key ������
	//! @param i Ԫ�ص�һά�±�
	//! @return >=:�ɹ�, -1:ʧ��
	public int CountArray2(string section, string key, int i)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key ������
			return -1;
		}

		if (v.type != ValueType.Array2)
		{
			// ֵ���ʹ���
			return -1;
		}

		int c1 = v.val_array2.Count;
		if ((i < 0) || (i >= c1))
		{
			// �±귶Χ����
			return -1;
		}

		ArrayList t = v.val_array2[i] as ArrayList;
		return t.Count;
	}

	//! ȡ��ά��������
	//! @param section ������
	//! @param key ������
	//! @param i Ԫ�ص�һά�±�
	//! @param j Ԫ�صڶ�ά�±�
	//! @param val [out]����ֵ
	//! @return not null:�ɹ�, null:ʧ��
	public string GetArray2(string section, string key, int i, int j)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key ������
			return null;
		}

		if (v.type != ValueType.Array2)
		{
			// ֵ���ʹ���
			return null;
		}

		int c1 = v.val_array2.Count;
		if ((i < 0) || (i >= c1))
		{
			// �±귶Χ����
			return null;
		}

		ArrayList t = v.val_array2[i] as ArrayList;

		int c2 = t.Count;
		if ((j < 0) || (j >= c2))
		{
			// �±귶Χ����
			return null;
		}

		return t[j] as string;
	}

	//! �޸ĵ�ֵ����
	//! @param section ������
	//! @param key ������
	//! @param val ����ֵ
	//! @return true:�ɹ�, false:ʧ��
	public bool SetSingle(string section, string key, string val)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key ������
			return false;
		}

		if (v.type != ValueType.Single)
		{
			// ֵ���ʹ���
			return false;
		}

		v.val_single = val;
		return true;
	}

	//! �޸�һά��������
	//! @param section ������
	//! @param key ������
	//! @param val ����ֵ
	//! @return true:�ɹ�, false:ʧ��
	public bool SetArray(string section, string key, ArrayList val)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key ������
			return false;
		}

		if (v.type != ValueType.Array)
		{
			// ֵ���ʹ���
			return false;
		}

		v.val_array = val;
		return true;
	}

	//! �޸Ķ�ά��������
	//! @param section ������
	//! @param key ������
	//! @param val ����ֵ
	//! @return true:�ɹ�, false:ʧ��
	public bool SetArray2(string section, string key, ArrayList val)
	{
		Value v = GetValue(section, key);
		if (v == null)
		{
			// key ������
			return false;
		}

		if (v.type != ValueType.Array2)
		{
			// ֵ���ʹ���
			return false;
		}

		v.val_array2 = val;
		return true;
	}

	//! �������ö�
	//! @param section ������
	//! @return not null:�ɹ�, null:ʧ��
	private Section GetSection(string section)
	{
		for (int i = 0; i < m_configure.Count; i++)
		{
			Section s = m_configure[i] as Section;
			if (s.section == section)
			{
				return s;
			}
		}

		return null;
	}

	//! ����������
	//! @param section ������
	//! @param key ������
	//! @return not null:�ɹ�, null:ʧ��
	private Value GetValue(string section, string key)
	{
		Section sec = GetSection(section);
		if (sec == null)
		{
			return null;
		}

		for (int i = 0; i < sec.values.Count; i++)
		{
			Value v = sec.values[i] as Value;
			if (v.key == key)
			{
				return v;
			}
		}

		return null;
	}

	//! �������ö�
	//! @param section ������
	//! @param comment ��ע��
	//! @return true:�ɹ�, false:ʧ��
	private bool AddSection(string section, string comment1, string comment2)
	{
		Section s = GetSection(section);
		if (s != null)
		{
			// section �Ѿ�����
			return false;
		}

		Section sec = new Section();
		sec.section = section;
		sec.comment1 = comment1;
		sec.comment2 = comment2;
		m_configure.Add(sec);
		return true;
	}

	//! ����������(��ֵ)
	//! @param section ������
	//! @param key ������
	//! @param value ������ֵ
	//! @param comment ��ע��
	//! @return true:�ɹ�, false:ʧ��
	private bool AddValueSingle(string section, string key, string value, string comment1, string comment2)
	{
		Section sec = GetSection(section);
		if (sec == null)
		{
			// section ������
			return false;
		}

		Value v = GetValue(section, key);
		if (v != null)
		{
			// key �Ѵ���
			return false;
		}

		Value val = new Value();
		val.key = key;
		val.type = ValueType.Single;
		val.val_single = value;
		val.comment1 = comment1;
		val.comment2 = comment2;

		sec.values.Add(val);
		return true;
	}

	//! ����������(һά����)
	//! @param section ������
	//! @param key ������
	//! @param line ������
	//! @param comment ��ע��
	//! @return true:�ɹ�, false:ʧ��
	private bool AddValueArray(string section, string key, string line, string comment1, string comment2)
	{
		Section sec = GetSection(section);
		if (sec == null)
		{
			// section ������
			return false;
		}

		Value v = GetValue(section, key);
		if (v != null)
		{
			// key �Ѵ���
			return false;
		}

		Value val = new Value();
		val.key = key;
		val.type = ValueType.Array;
		val.comment1 = comment1;
		val.comment2 = comment2;

		// parse line
		string l = line.Replace('\t', ' ');

		while (true)
		{
			l = l.Trim();

			int pos = l.IndexOf(' ');
			if (pos != -1)
			{
				string item = l.Substring(0, pos);
				l = l.Substring(pos + 1);
				val.val_array.Add(item);
			}
			else
			{
				l = l.Trim();
				if (l.Length > 0)
				{
					val.val_array.Add(l);
				}
				break;
			}
		}

		sec.values.Add(val);
		return true;
	}

	//! ����������(��ά����)
	//! @param section ������
	//! @param key ������
	//! @param lines ���������
	//! @param comment ��ע��
	//! @return true:�ɹ�, false:ʧ��
	private bool AddValueArray2(string section, string key, ArrayList lines, string comment1, string comment2)
	{
		Section sec = GetSection(section);
		if (sec == null)
		{
			// section ������
			return false;
		}

		Value v = GetValue(section, key);
		if (v != null)
		{
			// key �Ѵ���
			return false;
		}

		Value val = new Value();
		val.key = key;
		val.type = ValueType.Array2;
		val.comment1 = comment1;
		val.comment2 = comment2;

		// parse lines
		for (int i = 0; i < lines.Count; i++)
		{
			ArrayList val_array = new ArrayList();

			string l = lines[i] as string;
			l = l.Replace('\t', ' ');

			while (true)
			{
				l = l.Trim();

				int pos = l.IndexOf(' ');
				if (pos != -1)
				{
					string item = l.Substring(0, pos);
					l = l.Substring(pos + 1);
					val_array.Add(item);
				}
				else
				{
					l = l.Trim();
					if (l.Length > 0) {
						val_array.Add(l);
					}
					break;
				}
			}

			val.val_array2.Add(val_array);
		}

		sec.values.Add(val);
		return true;
	}
}

