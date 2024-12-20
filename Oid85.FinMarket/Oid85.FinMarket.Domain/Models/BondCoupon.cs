﻿namespace Oid85.FinMarket.Domain.Models;

public class BondCoupon
{
    /// <summary>
    /// Тикер
    /// </summary>
    public string Ticker { get; set; }
    
    /// <summary>
    /// Дата выплаты купона
    /// </summary>
    public DateOnly CouponDate { get; set; }
    
    /// <summary>
    /// Номер купона
    /// </summary>
    public long CouponNumber { get; set; }
    
    /// <summary>
    /// Купонный период в днях
    /// </summary>
    public int CouponPeriod { get; set; }
    
    /// <summary>
    /// Начало купонного периода
    /// </summary>
    public DateOnly CouponStartDate { get; set; }
    
    /// <summary>
    /// Окончание купонного периода
    /// </summary>
    public DateOnly CouponEndDate { get; set; }
    
    /// <summary>
    /// Выплата на одну облигацию
    /// </summary>
    public double PayOneBond { get; set; }
}