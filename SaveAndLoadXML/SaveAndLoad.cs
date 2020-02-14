using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;


public class SaveAndLoad : MonoBehaviour
{
    private List<BookModel> mBookModelList = new List<BookModel>();
    
    public void Save()
    {
        
    }

    public void Load()
    {
        //使用XMLDocument
        XmlDocument doc = new XmlDocument();
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;  //忽略XML文档里面的注释
        XmlReader reader = XmlReader.Create(@"...\XML",settings);
        doc.Load(reader);
        reader.Close();

        XmlNode node = doc.SelectSingleNode("bookstore");
        XmlNodeList childNodes = node.ChildNodes;

        foreach (var childNode in childNodes)
        {
            BookModel bookModel = new BookModel();
            //将节点转换为元素，便于得到节点的属性值
            XmlElement xe = (XmlElement) childNode;
            //得到Type和ISBN两个属性的属性值
            bookModel.BookType = xe.GetAttribute("Type");
            bookModel.BookISBN = xe.GetAttribute("ISBN");

            //得到book节点下的所有子节点
            XmlNodeList xnl = xe.ChildNodes;
            bookModel.BookName = xnl.Item(0)?.InnerText;
            bookModel.BookAuthor = xnl.Item(1)?.InnerText;
            bookModel.BookPrice = Double.Parse(xnl.Item(3)?.InnerText);
            this.mBookModelList.Add(bookModel);
        }

    }

    public void AddNode()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"...\XML");
        XmlNode node = doc.SelectSingleNode("bookstore");
        
        //创建一个节点，并设置节点的属性
        XmlElement eKey = doc.CreateElement("book");
        XmlAttribute aType = doc.CreateAttribute("Type");
        aType.InnerText = "sddsfs";
        eKey.SetAttributeNode(aType);
        //创建子节点
        XmlElement eAuthor = doc.CreateElement("author");
        eAuthor.InnerText = "sdsds";
        eKey.AppendChild(eAuthor);
        //最后把book节点挂接在要节点上,并保存整个文件
        node.AppendChild(eKey);
        doc.Save(@"...\XML");
    }

    /// <summary>
    /// 删除某条数据
    /// </summary>
    public void RemoveNode()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"...\XML");
        XmlNode node = doc.SelectSingleNode("bookstore");
        
        //想要删除某个几点，直接找到其父节点，然后调用RemoveChild方法
        XmlElement eKey = doc.DocumentElement;  //DocumentElement获取xml文档对象的根XmlElement.
        string strPath = string.Format("/bookstor/book[@ISBN=\"{0}\"]",mBookModelList[0].BookISBN);
        XmlElement selectEkey = (XmlElement)eKey.SelectSingleNode(strPath);  //SelectSingleNode根据表达式，获得符合条件的第一个节点
        selectEkey.ParentNode.RemoveChild(selectEkey);
        
        doc.Save(@"...\XML");
    }

    /// <summary>
    /// 修改某条数据
    /// </summary>
    public void ChangeNode()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"...\XML");
        XmlNode node = doc.SelectSingleNode("bookstore");

        XmlElement eKey = doc.DocumentElement;
        string strPath = string.Format("/bookstor/book[@ISBN=\"{0}\"]",mBookModelList[0].BookISBN);
        XmlElement selectEkey = (XmlElement)eKey.SelectSingleNode(strPath);  //SelectSingleNode根据表达式，获得符合条件的第一个节点
        selectEkey.SetAttribute("Type", mBookModelList[0].BookType); //也可以通过SetAttribute来增加一个属性
        selectEkey.GetElementsByTagName("author").Item(0).InnerText = mBookModelList[0].BookAuthor;
        
        doc.Save(@"...\XML");
    }
    
    
    //使用XMLTextReader和XMLTextWriter
    //XMLTextReader和XMLTextWriter是以流的形式来读写XML文件

    public object XMLTextReaderSave()
    {
        //使用XMLTextReader读取数据的时候，
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(@"...\XML");

        XmlNode node = xmlDoc.SelectSingleNode("节点名字");
        string xmlSerializerString = node.InnerXml;
        
        //把字符串改成二进制
        UTF8Encoding encoding = new UTF8Encoding();
        byte[] byteArray = encoding.GetBytes(xmlSerializerString);
        
        XmlSerializer serializer = new XmlSerializer(typeof(BookModel));
        MemoryStream memoryStream = new MemoryStream(byteArray);
        object obj = serializer.Deserialize(memoryStream);

        return obj;
    }
    
    /// <summary>
    /// 
    /// </summary>
    public void XMLTextReaderLoad()
    {
        //使用XMLTextWriter写入数据的时候，创建一个流
        XmlTextWriter Writer = new XmlTextWriter(@"...\XML",Encoding.UTF8);
        BookModel model = new BookModel();
        
        XmlSerializer serializer = new XmlSerializer(typeof(BookModel));
        MemoryStream memoryStream = new MemoryStream();
        XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream,Encoding.UTF8);
        serializer.Serialize(xmlTextWriter, model);
        memoryStream = (MemoryStream) xmlTextWriter.BaseStream;
        
        //把二进制转成字符串
        UTF8Encoding encoding = new UTF8Encoding();
        string eString = encoding.GetString(memoryStream.ToArray());
        
        Writer.WriteRaw(eString);
        xmlTextWriter.Close();
        Writer.Close();
    }
    
    
}
