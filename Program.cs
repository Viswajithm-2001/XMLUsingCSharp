using System;
using System.Xml;
using System.Collections.Generic;
namespace XML_Prac;

public class FoodDetails
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public int Calories { get; set; }

    //Using XML node
    public void FDReadFromXML(XmlNode fdNode)
    {
        XmlNode nameNode = fdNode["name"];
        XmlNode priceNode = fdNode["price"];
        XmlNode descriptionNode = fdNode["description"];
        XmlNode caloriesNode = fdNode["calories"];
        if(nameNode !=null && priceNode !=null && descriptionNode!=null && caloriesNode != null)
        {
            Name = nameNode.InnerText;
            Price = double.Parse(priceNode.InnerText.Trim('$'));
            Description = descriptionNode.InnerText;
            Calories = int.Parse(caloriesNode.InnerText);
        }
        else{
            return;
        }

    }
    
    //using XML Reader
    public void FoodDetailsReadFromXML(XmlReader foodDetailsReader)
    {
        foodDetailsReader.ReadToFollowing("name");
        Name = foodDetailsReader.ReadElementContentAsString();
        foodDetailsReader.ReadToFollowing("price");
        Price = double.Parse(foodDetailsReader.ReadElementContentAsString().Trim('$'));
        foodDetailsReader.ReadToFollowing("description");
        Description = foodDetailsReader.ReadElementContentAsString();
        foodDetailsReader.ReadToFollowing("calories");
        Calories = int.Parse(foodDetailsReader.ReadElementContentAsString());
    }
    public void WriteToXML()
    {

    }
}
public class Food
{
    public int OrderID { get; set; }
    public FoodDetails FoodDetails { get; set; } = new FoodDetails();

    //Using XML node
    public void FReadFromXML(XmlNode fdNode)
    {
        if(fdNode.Attributes["orderid"]!=null)
        {
            // FoodDetails foodDetails = new FoodDetails();
            OrderID = int.Parse(fdNode.Attributes["orderid"].Value);
            FoodDetails.FDReadFromXML(fdNode);
            // FoodDetails = foodDetails;
        }
        else
        {
            return;
        }
    }
    
    //using XML reader
    public void FoodReadFromXML(XmlReader foodReader)
    {
        if(foodReader.HasAttributes)
        {
            OrderID = int.Parse(foodReader.GetAttribute("orderid"));
            FoodDetails.FoodDetailsReadFromXML(foodReader);

        }
        else
        {
            return;
        }
    }
    public void WriteToXML()
    {

    }
}
public class LunchMenu
{
    public List<Food> Foods { get; set; } = new List<Food>();

    //using XML Node
    public void LunchMenuReadFromXML(XmlNode root)
    {
        // XmlNode root = doc.DocumentElement;
        if (root.Name != "lunch_menu")
        {
            System.Console.WriteLine("Invalid");
            return;
        }
        foreach(XmlNode node in root.ChildNodes)
        {
            Food food = new Food();
            food.FReadFromXML(node);
            Foods.Add(food);
        }
        // Food.FReadFromXML(root);

    }
    
    //using XML reader
    public void LunchMenuReadFromXML(XmlReader reader)
    {
        while(reader.ReadToFollowing("food"))
        {
            Food food = new Food();
            food.FoodReadFromXML(reader);
            Foods.Add(food);
        }
    }
    public void WriteToXML()
    {

    }
}

class Program
{
    static LunchMenu lunchMenu = new LunchMenu();
    public static void Main(string[] args)
    {
        string filePath = "FoodCatelog3.xml"; // Path to your XML file

        

        ReadFromXML(filePath);

    }
    public static void ReadFromXML(string path)
    {
        // XmlDocument doc = new XmlDocument();
        // doc.Load(path);
        // XmlNode root = doc.DocumentElement;
        // if(root==null || root.Name != "lunch_menu")
        // {
        //     System.Console.WriteLine("Invalid");
        //     return;
        // }
        // lunchMenu.LunchMenuReadFromXML(root);
        
        using(XmlReader reader = XmlReader.Create(path))
        {
            reader.ReadToFollowing("lunch_menu");
            lunchMenu.LunchMenuReadFromXML(reader);
        }

        System.Console.WriteLine("success");

        foreach (Food food in lunchMenu.Foods)
        {
            Console.WriteLine($"OrderId: {food.OrderID}, Name: {food.FoodDetails.Name}, Price: {food.FoodDetails.Price}, Description: {food.FoodDetails.Description}, Calories: {food.FoodDetails.Calories}");
        }

    }
    public static void WriteToXml(string filePath)
    {
        XmlWriterSettings settings = new XmlWriterSettings
        {
            Indent = true
        }; // for indentation and proper indentation

        // Create an XmlWriter for writing to the file
        using (XmlWriter writer = XmlWriter.Create(filePath, settings))
        {
            // Write the start element for the dinner menu
            writer.WriteStartElement("lunch_menu");

            // Write each food item
            foreach (Food food in lunchMenu.Foods)
            {
                writer.WriteStartElement("food");
                writer.WriteAttributeString("order", food.OrderID.ToString());
                writer.WriteElementString("name", food.FoodDetails.Name);
                writer.WriteElementString("price", food.FoodDetails.Price.ToString("C")); // Format price as currency
                writer.WriteElementString("description", food.FoodDetails.Description);
                writer.WriteElementString("calories", food.FoodDetails.Calories.ToString());
                writer.WriteEndElement(); // Close food element
            }
            writer.WriteEndElement();//closes lunch_menu
            Console.WriteLine("Xml file created");
            writer.Close();//close file
        }
    }
}