using System.Runtime.CompilerServices;
using NLog;

namespace Oid85.FinMarket.Common.Logger;

/// <summary>
/// Логер проекта
/// </summary>
public class FinMarketLogger(
    ILogger logger)
{
    public void LogError(
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string memberName = "") =>
        logger.Error(
            message + ". Debug info: {filePath}, {lineNumber}, {memberName}", 
            filePath, lineNumber, memberName);

    public void LogError(
        Exception exception, 
        string message,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0,
        [CallerMemberName] string memberName = "") =>
        logger.Error(
            exception, 
            message + ". Debug info: {filePath}, {lineNumber}, {memberName}", 
            filePath, lineNumber, memberName);

    public void LogInfo(string message) =>
        logger.Info(message);
}