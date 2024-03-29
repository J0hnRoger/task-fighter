﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentAssertions;
using MediatR;
using TaskFighter.Domain.DayScheduling.Requests;
using TaskFighter.Domain.TasksManagement.Requests;
using TaskFighter.Infrastructure.CommandParsing;
using TaskFighter.Infrastructure.Configuration;
using Xunit;

namespace TaskFighter.Tests;

public class CommandLineParserTests
{
    private static CommandParser CreateCommandParser()
    {
        var allRequestTypes = typeof(CommandParser)
            .Assembly
            .GetTypes()
            .Where(t => t.GetInterfaces().Contains(typeof(IBaseRequest)))
            .ToList();
        
        CommandParser parser = new CommandParser(allRequestTypes);
        return parser;
    }
    
    [Fact]
    public void CommandLineParser_ReturnNotFoundRequest_WhenUnknowArgs()
    {
        CommandParser parser = CreateCommandParser();
        var result = parser.ParseArgs("t dontexist Nettoyer le poêle a:majordome");
        result.Should().BeOfType<NotFoundRequest>();
    }
    
    [Fact]
    public void CommandLineParser_ReturnTaskEntity()
    {
        CommandParser parser = CreateCommandParser();
        var taskRequest = parser.ParseArgs("task +home add Nettoyer le poêle");
        taskRequest.Should().BeOfType<AddTaskRequest>();
        
        var dayRequest = parser.ParseArgs("day list");
        dayRequest.Should().BeOfType<ListDayRequest>();
        
        var configRequest = parser.ParseArgs("config show");
        configRequest.Should().BeOfType<ShowConfigRequest>();
    }
    
    [Fact]
    public void CommandLineParser_ReturnFiltersEntity()
    {
        CommandParser parser = CreateCommandParser();
        var request = parser.ParseArgs("task +home +bricolage p:backlog list");
        request.Should().BeOfType<ListTaskRequest>();
        var taskRequest = request as ListTaskRequest;
        taskRequest!.Tags.Should().HaveCount(2);
        taskRequest.Tags[0].Should().Be("home");
        taskRequest.Tags[1].Should().Be("bricolage");
        taskRequest.Project.Should().Be("");
    }

    [Fact]
    public void ParsingRegex_ParseCommand_Without_Filters()
    {
        string parsingRegex = @"(\w*) (\S* )*\b(add|modify|delete|list)\b( .*)";
        string input = "task add \"Tester la regex du SamuraiTask\" c:perso a:mindset";
        var result = Regex.Match(input, parsingRegex);
        result.Groups.Count.Should().Be(5);
        result.Groups[0].Value.Should().Be(input);
        result.Groups[1].Value.Should().Be("task");
        result.Groups[2].Value.Should().Be("");
        result.Groups[3].Value.Should().Be("add");
        result.Groups[4].Value.Should().Be(" \"Tester la regex du SamuraiTask\" c:perso a:mindset");
    }

    [Fact]
    public void ParsingRegex_ParseCommand_WithFilters()
    {
        string parsingRegex = @"(\w*) (\S)*\s* \b(add|modify|delete|list)\b (.*)";
        string input = "task 1 modify \"Tester la regex du SamuraiTask\"";
        var result = Regex.Match(input, parsingRegex);
        result.Groups.Count.Should().Be(5);
        result.Groups[0].Value.Should().Be(input);
        result.Groups[1].Value.Should().Be("task");
        result.Groups[2].Value.Should().Be("1");
        result.Groups[3].Value.Should().Be("modify");
        result.Groups[4].Value.Should().Be("\"Tester la regex du SamuraiTask\"");
    }

    [Fact]
    public void CommandParser_Parse_Without_Filters()
    {
        string parsingRegex = @"(\w*) (\S)*\s* \b(add|modify|delete|list)\b (.*)";
        CommandParser parser = CreateCommandParser();
        string input = "task add \"Tester la regex du SamuraiTask\" c:perso a:mindset";
        var result = Regex.Match(input, parsingRegex);
    }

    [Fact]
    public void CommandLineParser_ParseArgs_IntoMediatrCommands()
    {
        // https://github.com/commandlineparser/commandline/issues/291

        // https://thirty25.com/posts/2020/09/commandline-mediatr
        CommandParser parser = CreateCommandParser();
        var result = parser.ParseArgs("t add Nettoyer le poêle a:majordome");
        //1. Parser l'Entity (obligatoire)
        // eg: task, event, day, log
        result.Should().BeOfType<AddTaskRequest>();
        //2. Parser les Filters (optionnel) - pour le render la plupart du temps 
        result = parser.ParseArgs("t add Nettoyer le poêle a:majordome");
        // eg: +home, status:done, priority:H
        //3. Parser la Command (obligatoire)
        // eg; create, update, delete, list, start, finish
        //4. Parser les Modifiers (optionnel) - pour 
        // eg: <modify> priority:H, startDate:today, due:19/08/2022, due:monday, due:+1
        result.Should().BeOfType<AddTaskRequest>();
    }

    [Fact]
    public void CommandLine_ParseCreateTasksCommand()
    {
        CommandParser parser = CreateCommandParser();
        var result = parser.ParseArgs("t add Nettoyer le poêle a:majordome");
        result.Should().BeOfType<AddTaskRequest>();
    }

    [Fact]
    public void CommandLine_ParseDeleteTasksCommand()
    {
        CommandParser parser = CreateCommandParser();
        var result = parser.ParseArgs("t delete Nettoyer le poêle a:majordome");
        result.Should().BeOfType<DeleteTaskRequest>();
    }
}