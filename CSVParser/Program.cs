using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace CSVParser
{
    class Program
    {
        List<string[]> csvData1 = new List<string[]>();
        // Stores the base path for where to find the financial data CSV files
        // const keyword means it cannot be changed once initialised
        const string fileBasePath = @"..\..\..\CSVFiles\";

        #region the struct that stores the column data for the file
        public class financial
        {
            // TODO modify it to work with all the fields needed in the financial data
            //public string field1;
            //public string field2;
            //public string field3;
            //public string field4;
            //public string field5;
            //public string field6;
            //public string field7;
            //public string field8;
            //public string field9;
            //public string field10;

            public string field1;
            public string field2;
            public uint field3;
            public uint field4;
            public uint field5;
            public uint field6;
            public ulong field7;
            public double field8;
            public double field9;
            public char field10;

            // Default Constructor for the struct
            public financial()
            {
                this.field1 = "";
                this.field2 = "";
                this.field3 = 0;
                this.field4 = 0;
                this.field5 = 0;
                this.field6 = 0;
                this.field7 = 0;
                this.field8 = 0.0;
                this.field9 = 0.0;
                this.field10 = '\0';
            }

            // The set(string[]) method sets values for the struct object based on line values passed as a parameter
            public static financial set(string[] values)
            {
                financial a = new financial();

                a.field1 = values[0];
                a.field2 = values[1];
                a.field3 = Convert.ToUInt16(values[2]);
                a.field4 = Convert.ToUInt16(values[3]);
                a.field5 = Convert.ToUInt16(values[4]);
                a.field6 = Convert.ToUInt16(values[5]);
                a.field7 = Convert.ToUInt64(values[6]);
                a.field8 = Convert.ToDouble(values[7]);
                a.field9 = Convert.ToDouble(values[8]);
                a.field10 = Convert.ToChar(values[9]);

                return a;
            }
        }
        #endregion 

        #region The getSplittedLine(string) method accepts a line of financial data and returns a parsed array of string data
        static string[] getSplittedLine(string lineOfData)
        {
            List<string> dataElements = new List<string>();
            string[] line = { };
            string[] financialData;
            
            // extract the fields
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            financialData = CSVParser.Split(lineOfData);

            // clean up the fields (remove " and leading spaces)
            for (int i = 0; i < financialData.Length; i++)
            {
                financialData[i] = financialData[i].TrimStart(' ', '"');
                financialData[i] = financialData[i].TrimEnd('"',' ');

                //Dont have a ',' after the C {"EQUITY_NAME"}
                /*if (i != 9)
                {
                    financialData[i] = financialData[i];
                }
                else
                {
                    //Nothing actually needs to be here 
                }*/
              
                #region print
                //if (i == 0)
                //{
                //    Console.WriteLine();
                //}
                //Console.Write(financialData[i]);
                #endregion
            }

            return financialData;
        }
        #endregion

        #region The getHeaderCSV(string[]) method provides accepts a line of metadata and returns the line in a formatted way
        static string getHeaderCSV(string[] line)
        {
            //dont know if this actually works
            string result = "";
            for (int i = 0; i < line.Length; i++)
            {
                result += line[i] + ',';
            }
            return result;

        }
        #endregion

        #region The getFinCSV(financial) method returns the line of financial data
        static string getFinCSV(financial a)
        {
            return a.field1 + ',' + a.field2 + ',' + a.field3 + ',' + a.field4 + ',' + a.field5 + ',' + a.field6 + ',' + a.field7 + ',' + a.field8 + ',' + a.field9 + ',' + a.field10;
        }
        #endregion

        #region The writeData(List<string[]>, List<struct>) method accepts header data and the financial data and writes them to a sanitised file
        static void writeData(string path, string[] header, List<financial> fin)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(getHeaderCSV(header));
            string toWrite = "";//string that will be written to the csv file
            for (int i = 0; i < fin.Count(); i++)
            {
                
                toWrite = getFinCSV(fin[i]);//put the data into a string to be further formatted
                //reader.WriteLine(getFinCSV(fin[i]));
                /*if (i<fin.Count())//as long as it is not at the end of the string it adds a comma dellimeter
                {
                    toWrite;
                }*/
                //toWrite = toWrite.Substring(0, (toWrite.Length - 1));//cuts the end off of the string to get rid of the ", " that gets added cuz i cant get rid of it.
                writer.WriteLine(toWrite);//write to the file
                
            }
            writer.Close();
        }

        #endregion

        #region The parseData(string, List<string[]>, List<string[]) method takes the path to the file and reads the file line by line, teasing out the header and actual data
        static void parseData(string path, string[] header, List<string[]> csvData)
        {
           // csvData = new List<string[]>();
            
            // create a stream reader to read from a csv file
            StreamReader reader = new StreamReader(path);

            // use this to read in each line
            string line = null;
            string[] dataElements = {};

            try
            {
                // loop through the file using some looping construct and appropriate exit condition
                // store the first line in a string header array
                int cnt = 0;
                int col = 0;

                line = reader.ReadLine();
                //Console.WriteLine("line = " + line); //line = ALPHA_CODE,EQUITY_NAME,YEAR,WEEK,ACTUAL_CLOSE,ACTUAL_VOLUME,MARKET_CAP,DIV_YIELD,PE_RATIO,EQUITY_STATUS
                
                header = line.Split(',');
                //Console.WriteLine("header[0] = " + header[0]); //header 1 = ALPHA_CODE 
                //Console.WriteLine("header[1] = " + header[1]); //header 2 = EQUITY_NAME

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    //Console.WriteLine("line " + line);
                    dataElements = getSplittedLine(line);
                    //Console.WriteLine("dataElements " + dataElements[j]);
                    //j++;

                    //if (cnt == 5)  //make sure counter doesn't go past 10 as 10 is the max field e.g. EQUITY_STATUS
                    //{
                    //    cnt = 0;
                    //    col++;          //when the end of the fields have been reached move onto the the col 
                    //}
                    //else
                    //{
                    //    // store each column of data in an index position in the csvData string list
                    //    csvData.Add(dataElements);  //Apparently this still doesn't work, data is stored here but does not transfer to main csvData
                    //    cnt++;
                    //}            
                    csvData.Add(dataElements);
                }
                //csvData DOES NOT STORE HEADERS

                #region print
                //for (int i = 0; i < header.Length; i++)
                //{
                //    Console.Write(header[i] + " ");
                //}
                //Console.WriteLine();

                //for (int i = 0; i < csvData.Count; i++)
                //{
                //    //            csvData[col][row]
                //    Console.Write(csvData[i][0]);
                //    Console.Write(csvData[i][1]);
                //    Console.Write(csvData[i][2]);
                //    Console.Write(csvData[i][3]);
                //    Console.Write(csvData[i][4]);
                //    Console.Write(csvData[i][5]);
                //    Console.Write(csvData[i][6]);
                //    Console.Write(csvData[i][7]);
                //    Console.Write(csvData[i][8]);
                //    Console.Write(csvData[i][9]);

                //    Console.WriteLine();
                //    //[0][0] = AFE {"ALPHA_CODE"}, [0][9] = C {"EQUITY_STATUS"}                   
                //}
                //Console.WriteLine();
                ////Console.Write("csvData " + csvData[0][0] + csvData[0][9]);
                #endregion

                // Output number of lines read
                Console.WriteLine("Read {0} lines of text!!!", csvData.Count);

            }
            catch (ArgumentException e)
            {

                Console.WriteLine("Unspecified Error. ", e);
            }
            catch (IOException io)
            {
                Console.WriteLine("IO Error. ", io);
                //TODO
            }
            finally
            {
                // always remember to close the file..
                reader.Close();
                // code in a finally clause will always be executed even if an exception is thrown
            }
        }
        #endregion

        #region The formatData(string) method returns the line properly formatted
        static string formatData(string field)
        {
            field = field.TrimEnd(' ', ','); //remove trailing chars so you are just left with data you want

            if(field.Contains(',')) //removes all commas from a number e.g. 202,323,342,123 ---> 202323342123 so it can be stored as int
            {
                while(field.Contains(','))
                {
                    field = field.Remove(field.IndexOf(','), 1);
                }         
            }
            if (field.Contains('.'))//this needs to be here because doubles are accepted with ',' not '.' correct format for double = 6,666
            {
                field = field.Replace('.', ',');
            }

            if (field.Equals("-"))//deals with erronous data/null values for numbers
            {
                field = "00";
            }

            return field;
        }
        #endregion

        #region Main(string[] args) This is the main method that runs the application
        static void Main(string[] args)
        {
            // compile the path to the sample file
            // this program gets the file name from the command line arguments 
            // Click Project > CVSParser Properties > Debug > Start Options > Command Line Arguments
            List<string[]> csvData = new List<string[]>();
            string[] header = new string[11];
            List<financial> finData = new List<financial>();
            for (int run = 0; run < 5; run++)
            {

                string fullPath = fileBasePath + args[run];

                // initialise the data structures required for the solution
                
                StreamReader reader = new StreamReader(fullPath);
                string line = null;
                line = reader.ReadLine();
                header = line.Split(',');
                reader.Close();

                // call the main parse method
                parseData(fullPath, header, csvData);

               

                for (int i = 0; i < csvData.Count; i++)
                {
                    financial a = new financial();

                   

                    //Make sure the headers aren't read into the fields moving from 1 csv file to the next
                    if (formatData(csvData[i][0]) == "ALPHA_CODE")
                    {
                        i++;
                    }

                    //csvData[col][row]
                    //temp is used to check values in error testing
                    //string temp = "";
                    //temp = formatData(csvData[i][*]);
                    a.field1 = csvData[i][0];
                    a.field2 = csvData[i][1];
                    a.field3 = Convert.ToUInt16(formatData(csvData[i][2]));
                    a.field4 = Convert.ToUInt16(formatData(csvData[i][3]));
                    a.field5 = Convert.ToUInt16(formatData(csvData[i][4]));
                    a.field6 = Convert.ToUInt32(formatData(csvData[i][5]));
                    a.field7 = Convert.ToUInt64(formatData(csvData[i][6]));
                    a.field8 = Convert.ToDouble(formatData(csvData[i][7]));
                    a.field9 = Convert.ToDouble(formatData(csvData[i][8]));
                    a.field10 = Convert.ToChar(formatData(csvData[i][9]));

                    // There are not 53 weeks in a year, if so add 1 to year and set the week to 1 (the first week of a new year)
                    if (a.field4 == 53 || a.field6 == 00)
                    {
                        //a.field3++;
                        //a.field4 = 1;
                        //finData.Add(a);
                        i++;
                    }
                    else
                    {
                        finData.Add(a);
                    }
                }

                #region Print
                //Console.WriteLine("Finished the loop");
                //Console.WriteLine("The 100th entry:");
                //Console.WriteLine();
                //for (int i = 0; i < 10; i++)
                //{

                //    if(i == 9)
                //    {
                //        Console.Write(header[i]);
                //    }
                //    else
                //    {
                //        Console.Write(header[i] + ", ");
                //    }
                //}
                //Console.WriteLine();
                //for (int i = 0; i < 10; i++)
                //{
                //    Console.Write(csvData[100][i]);
                //}
                #endregion

                //ToDo questions...

                // make the console read a character followed by ENTER from the keyboard
                // this so that it does not close abruptly before we see the output
                
                
            }
            writeData(fileBasePath + "filedata.csv", header, finData);
            Console.WriteLine("Finished.");
            Console.Read();
        }
        #endregion
    }
}