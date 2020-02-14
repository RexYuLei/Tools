using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookModel
{
    
    /// <summary>
    /// 对应的课程类型
    /// </summary>
    private string mBookType;
    public string BookType
    {
        get => mBookType;
        set => mBookType = value;
    }

    /// <summary>
    /// 对应的ISBN号
    /// </summary>
    private string mBookISBN;
    public string BookISBN
    {
        get => mBookISBN;
        set => mBookISBN = value;
    }

    /// <summary>
    /// 书名
    /// </summary>
    private string mBookName;
    public string BookName
    {
        get => mBookName;
        set => mBookName = value;
    }

    /// <summary>
    /// 作者
    /// </summary>
    private string mBookAuthor;
    public string BookAuthor
    {
        get => mBookAuthor;
        set => mBookAuthor = value;
    }

    /// <summary>
    /// 价格
    /// </summary>
    private Double mBookPrice;
    public Double BookPrice
    {
        get => mBookPrice;
        set => mBookPrice = value;
    }
}
