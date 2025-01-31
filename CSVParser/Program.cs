﻿using System;
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

        public static int stockClose(string code, int year, List<financial> finData, string path)
        {
            StreamReader sr = new StreamReader(path + "filedata.csv");
            string[] forerunner = { };
            string[] dataElements = new string[10];
            while (!sr.EndOfStream)
            {
                forerunner = sr.ReadLine().Split(',');
                
                
                string yr = "" + year;

                if (dataElements[0]==code && dataElements[2]==yr)
                {
                    if (Convert.ToInt32(forerunner[3]) < Convert.ToInt32(dataElements[3]))
                    {
                        return Convert.ToInt32(dataElements[4]);//return the closing stock
                    }
                    
                }
                dataElements = forerunner;
            }
            return 0;
        }

        public static int totalShareSales(string code, int year, List<financial> finData, string path)
        {
            StreamReader sr = new StreamReader(path + "filedata.csv");
            string[] forerunner = { };
            string[] dataElements = new string[10];
            while (!sr.EndOfStream)
            {
                forerunner = sr.ReadLine().Split(',');


                string yr = "" + year;

                if (dataElements[0] == code && dataElements[2] == yr)
                {
                    if (Convert.ToInt32(forerunner[3]) < Convert.ToInt32(dataElements[3]))
                    {
                        return Convert.ToInt32(dataElements[5]);//return the closing stock
                    }
                }
                dataElements = forerunner;
            }
            return 0;
        }

        public static void top5shares(int year, List<financial> finData, string path)
        {
            StreamReader sr = new StreamReader(path + "filedata.csv");
            string[] dataElements = new string[10];
            List<string[]> div = new List<string[]>();
            while (!sr.EndOfStream)
            {
                dataElements = sr.ReadLine().Split(',');
                
                if(dataElements[2] == "2014")
                {
                    div.Add(dataElements);
                }
            }

            List<double> orderedDiv = new List<double>();//set initial values
            List<string> stockName = new List<string>();//set initial values

            for (int i = 0; i < div.Count; i++)
            {   
                orderedDiv.Add(Convert.ToInt32(div[i][7]));              
            }

            orderedDiv = orderedDiv.OrderByDescending(i => i).ToList();     //Order with biggest numbers at begining
            orderedDiv = orderedDiv.Distinct().ToList();    //remove duplicates

            for (int i = 0; i < orderedDiv.Count; i++)    //loop for the ammount of ordered div
            {
                for (int j = 0; j < div.Count; j++)     //loop for the ammounts of total entries for div 
                {
                    //if ordered div == div from complete entry
                    if (orderedDiv[i] == Convert.ToDouble(div[j][7])) 
                    {
                        stockName.Add(div[j][0]);       //take the name the div belongs too and add to list 0-1-2
                    }
                }  
            }
            
            for (int i = 0; i < orderedDiv.Count; i++) 
            {
                for (int j = 0; j < orderedDiv.Count; j++)
                {
                    if (stockName[i] == stockName[j+1])
                    {
                        stockName.RemoveAt(j+1);
                        orderedDiv.RemoveAt(j+1);
                    }
                }
            }

            Console.WriteLine("Top 5 stocks in terms of div yield in " + year);
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Number " + (i + 1) + " Stock is " + stockName[i] + " at a value of " + orderedDiv[i]);
            }

        }

        public static string marketCap(int year, List<financial> finData, string path)
        {
            StreamReader sr = new StreamReader(path + "filedata.csv");

            List<string[]> data = new List<string[]>();
            string[] dataElements = new string[10];
            string[] forerunner = new string[10];
            
            
            int pos = 0;//keeps track of the current value we want to be incrementing
            while (!sr.EndOfStream)
            {
                forerunner = sr.ReadLine().Split(',');
                string yr = "" + year;
                string[] temp = new string[2];
                Int64 temp1 = 0;

                if (yr == dataElements[2])//if the correct year 
                {
                    temp[0] = forerunner[0];
                    temp[1] = forerunner[6];
                    data.Add(temp);

                    if (forerunner[0] != dataElements[0])//if all the market caps have been added for a certain stock, move on to the next place in the List.
                    {
                        pos++;
                        temp[0] = forerunner[0];
                        temp[1] = forerunner[6];
                        data.Add(temp);
                    }
                    else
                    {
                        temp1 = Convert.ToInt64(data[pos][1]);
                        temp1 += Convert.ToInt64(dataElements[6]);
                        data[pos][1] = "" + temp1;//increment the market cap total associated with the current stock
                    }          
                }

                dataElements = forerunner;
            }
            string biggestcapname = data[0][0];//initialise the values
            string biggestcapval = data[0][1];

            for (int i = 0; i < data.Count; i++)
            {
                if (Convert.ToInt64(data[i][1])>Convert.ToInt64(biggestcapval))//if a bigger value is found then replace it with the bigger value
                {
                    biggestcapval = data[i][1];
                    biggestcapname = data[i][0];
                }
            }
            return "The stock with the highest revenue based on market cap is " + biggestcapname + " with a total market cap of: " + biggestcapval + ".";
        }

        #region the struct that stores the column data for the file
        public class financial
        {
            // TODO modify it to work with all the fields needed in the financial data
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
            }

            return financialData;
        }
        #endregion

        #region The getHeaderCSV(string[]) method provides accepts a line of metadata and returns the line in a formatted way
        static string getHeaderCSV(string[] line)
        {
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
                writer.WriteLine(toWrite);//write to the file
            }
            writer.Close();
        }

        #endregion

        #region The parseData(string, List<string[]>, List<string[]) method takes the path to the file and reads the file line by line, teasing out the header and actual data
        static void parseData(string path, string[] header, List<string[]> csvData)
        {
            // create a stream reader to read from a csv file
            StreamReader reader = new StreamReader(path);

            // use this to read in each line
            string line = null;
            string[] dataElements = {};

            try
            {
                // loop through the file using some looping construct and appropriate exit condition
                // store the first line in a string header array

                line = reader.ReadLine();
                //Console.WriteLine("line = " + line); //line = ALPHA_CODE,EQUITY_NAME,YEAR,WEEK,ACTUAL_CLOSE,ACTUAL_VOLUME,MARKET_CAP,DIV_YIELD,PE_RATIO,EQUITY_STATUS
                
                header = line.Split(',');
                //Console.WriteLine("header[0] = " + header[0]); //header 1 = ALPHA_CODE 
                //Console.WriteLine("header[1] = " + header[1]); //header 2 = EQUITY_NAME

                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();
                    dataElements = getSplittedLine(line);                  
                    csvData.Add(dataElements);
                }
                //csvData DOES NOT STORE HEADERS

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

            if (field.Contains(',')) //removes all commas from a number e.g. 202,323,342,123 ---> 202323342123 so it can be stored as int
            {
                while (field.Contains(','))
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
                field = ""+01;
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
                csvData = new List<string[]>();
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
                    if (a.field4 == 53 || a.field6 == 01)
                    {
                        i++;
                    }
                    else
                    {
                        finData.Add(a);
                    }
                }
                // make the console read a character followed by ENTER from the keyboard
                // this so that it does not close abruptly before we see the output
            }

            writeData(fileBasePath + "filedata.csv", header, finData);
            Console.WriteLine("Finished.");
            Console.WriteLine("YRK Stock Close: " + stockClose("YRK", 2014, finData, fileBasePath));
            Console.WriteLine("YRK Total Share Sales for final week: " + totalShareSales("YRK", 2014, finData, fileBasePath));
            top5shares(2012, finData, fileBasePath);
            Console.WriteLine(marketCap(2013, finData, fileBasePath));
            Console.Read();
        }
        #endregion
    }
}