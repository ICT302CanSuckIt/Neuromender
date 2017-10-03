using System;
using System.Xml;
using UnityEngine;

using MySql.Data.MySqlClient;
using UnityEngine.UI;

public class database : MonoBehaviour {


    //OLD SERVER private string connStr = "server=ref.servebeer.com;user=root;database=Neuromend;port=3306;password=Neuromend;";
    //private string connStr = "server=vegas.murdoch.edu.au;user=neuroadmin2_2;database=Neuromend2_2;port=3306;password=3f93acda3fd2a8088f2a84fdb47fd1a4947788a4;";
    //private string connStr = "server=localhost;user=root;database=team30_test;port=3306;password=mysql;";
    //private string connStr = "server=vegas.murdoch.edu.au;user=neuroadmin3;database=Neuromender3;port=3306;password=90E7CF21C7B16153FF503862D335F909E5AED943;";
    //private string connStr = "server=localhost;user=root;database=Neuromender3;port=3306;password=mysql;";



    //private string connStr = "server=192.168.0.102;user=root;database=neuromender3;port=3306;password=pwd4vr;";
     private string connStr = "server=localhost;user=root;database=neuromender3;port=3306;password=pwd4vr;";
    //private string connStr = "server=vegas.murdoch.edu.au;user=neuroadmin4;database=Neuromender4;port=3306;password=*7BA7FB152973F6C4434A6BC54E94A7366969EDFE;";


    private MySqlConnection conn;// = new MySqlConnection(connStr);
	public XmlDocument databaseXml;
	Text outputText;
    public bool connected = false;
	
	public bool LoginMenu;
	
	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		outputText = GameObject.Find("DatabaseOutputText").GetComponent<Text>();
		ConnectToDatabase();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	/// <summary>
	/// This function connects to the database!
	/// </summary>
	/// <returns></returns>
	public bool ConnectToDatabase ()
	{
		
		//print("Connecting to MySQL...");
		conn = new MySqlConnection(connStr);
		try{
			conn.Open();
		}
		catch (Exception ex)
		{
			if (outputText != null)
                outputText.text = "Output: Error connecting to the database.";

            print("Output: connecting " + ex.ToString());
            connected = false;
			return false;
		}
        connected = true;
		return true;
	}

    /// <summary>
    /// Check the connection to the external database. Should be done every time a database operation is required.
    /// </summary>
    /// <returns></returns>
    public bool PingConnection()
    {
        connected = conn.Ping();
        return connected;
    }
	
	public void CloseConnectionToDatabase ()
	{
		//conn.Close();
		//print("Done.");
	}
	
	/// <summary>
	/// Single line insert statement for mySQL
	/// </summary>
	/// <returns><c>true</c>, if data was submited, <c>false</c> otherwise.</returns>
	/// <param name="table">Table.</param>
	/// <param name="fields">Fields.</param>
	/// <param name="values">Values.</param>
	public bool SubmitData (string table, string fields, string values)
	{
		//ConnectToDatabase();
		
		string sql = "INSERT INTO %TABLE% (%FIELDS%) Values (%VALUES%)";
		try
		{ 
			
			sql = sql.Replace("%TABLE%", table);
			sql = sql.Replace("%FIELDS%", fields);
			sql = sql.Replace("%VALUES%", values);			
			MySqlCommand cmd = new MySqlCommand(sql, conn);
			cmd.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			if (outputText != null)
				outputText.text = "Output: " + ex.ToString();
			
			print("Output: Error " + ex.ToString());
			print("SQL: " + sql);
			return false;
		}
		//print("Done.");
		return true;
	}
	
	/// <summary>
	/// Miltiple line insert statement for mysql
	/// </summary>
	/// <returns><c>true</c>, if data milti line was submited, <c>false</c> otherwise.</returns>
	/// <param name="table">Table.</param>
	/// <param name="fields">Fields.</param>
	/// <param name="values">Values.</param>
	public bool SubmitDataMiltiLine (string table, string fields, string values)
	{
		/*ConnectToDatabase(); */
		
		string sql = "INSERT INTO %TABLE% (%FIELDS%) Values %VALUES%";
		try
		{ 
			
			sql = sql.Replace("%TABLE%", table);
			sql = sql.Replace("%FIELDS%", fields);
			sql = sql.Replace("%VALUES%", values);			
			MySqlCommand cmd = new MySqlCommand(sql, conn);
			cmd.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			if (outputText != null)
				outputText.text = "Output: " + ex.ToString();
			
			print("Output: Error " + ex.ToString());
			print("SQL: " + sql);
			return false;
		}
		//print("Done.");
		return true;
	}
	
	/// <summary>
	/// Update Existing data in the database (used mostly for sessions)
	/// </summary>
	/// <returns><c>true</c>, if data was updated, <c>false</c> otherwise.</returns>
	/// <param name="table">Table.</param>
	/// <param name="UpdateFields">Update fields.</param>
	/// <param name="Where">Where.</param>
	public bool UpdateData(string table, string UpdateFields, string Where)
	{
		/*ConnectToDatabase(); */
		
		string sql = "UPDATE %TABLE% SET %UPDATEFIELDS% WHERE %WHEREVALS%";
		try
		{
			
			sql = sql.Replace("%TABLE%", table);
			sql = sql.Replace("%UPDATEFIELDS%", UpdateFields);
			sql = sql.Replace("%WHEREVALS%", Where);
			print("SQL: " + sql);
			MySqlCommand cmd = new MySqlCommand(sql, conn);
			cmd.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			if (outputText != null)
				outputText.text = "Output: " + ex.ToString();
			
			print("Output: Error " + ex.ToString());
			print("SQL: " + sql);
			return false;
		}
		//print("Done.");
		return true;
	}
	
	/// <summary>
	/// Query the database for values (used mostly for login and userconfig)
	/// </summary>
	/// <returns>The data.</returns>
	/// <param name="sql">Sql.</param>
	public MySqlDataReader selectData(string sql)
	{
		MySqlCommand cmd = new MySqlCommand(sql, conn);
		MySqlDataReader returnValue = null;
		try
		{
			returnValue = cmd.ExecuteReader();
		}
		catch (Exception ex)
		{
			if (outputText != null)
				outputText.text = "Output: Error searching database";
			
			print("Output: Error " + ex.ToString());
			print("SQL: " + sql);
		}
		
		return returnValue;
	}
	
    /// <summary>
    /// Get the database time.
    /// </summary>
    /// <returns>The timestamp of the database as a DateTime object.</returns>
    public DateTime getDatabaseTime()
    {
        if(PingConnection())
        {
            MySqlCommand cmd = new MySqlCommand("SELECT CURRENT_TIMESTAMP", conn);
            MySqlDataReader returnValue = null;
            DateTime returnVal = DateTime.Now;

            try
            {
                returnValue = cmd.ExecuteReader();
                returnValue.Read();

                returnVal = returnValue.GetDateTime("CURRENT_TIMESTAMP");
            }
            catch(Exception ex)
            {
                Debug.Log("Error: "+ ex.Message + ". Local clock used instead.");
                returnVal = DateTime.Now;
            }

            cmd = null;
            returnValue.Close();

            return returnVal;
        }
        else
        {
            Debug.Log("No connection. Local clock used instead.");
            return DateTime.Now;
        }
    }
}
