using System.Collections.Generic;
using CSharpFunctionalExtensions;
using FluentAssertions;
using TaskSamurai.Domain.DayScheduling;
using TaskSamurai.Domain.EventsManagement;
using TaskSamurai.Domain.RitualsManagement;
using TaskSamurai.Domain.TasksManagement;
using TaskSamurai.Infrastructure.Persistence;
using Xunit;

namespace TaskFighter.Tests;

public class TaskFighterTests
{
    /// <summary>
    /// L'enjeu est double:
    /// - Me fournir la liste de tâches les plus impoctantes que je dois faire dans la journée pour me rapprocher de mes
    /// objectifs long-termes
    /// - Me fournir une overview exhaustive de ce que je dois faire sur toute ma journée, entre Event, Routines et Tasks impactantes
    /// Idée de **Crafter** sa journée pour trouver le meilleur agencement
    /// </summary>
    [Fact]
    public void TaskFighter_DayScheduleTemplate_Tests()
    {
        string configDir = TestConfiguration.GetSecretValue("ConfigPath");
        string daySchedulePath = configDir + "schedules.json";
        DayScheduleRepository repository = new DayScheduleRepository(daySchedulePath, configDir);
        DaySchedule template = repository.GetTemplate();
        template.TimeBlocks.Should().HaveCount(0);

        DaySchedule testDay = CreateWorkDaySchedule();
        repository.UpdateTemplate(testDay);
    }
    
    
    
    [Fact]
    public void TaskFighter_DaySchedule_Tests()
    {
        string configDir = TestConfiguration.GetSecretValue("ConfigPath");
        string daySchedulePath = configDir + "schedules.json";
        DayScheduleRepository repository = new DayScheduleRepository(daySchedulePath, configDir);
        var dateFormat = "24-12-2022";
        Result<DaySchedule> templateResult = repository.Get(dateFormat);
        templateResult.IsSuccess.Should().Be(false); 
        
        DaySchedule template = CreateWorkDaySchedule();
        repository.Create(dateFormat, template); 
         
        template.TimeBlocks.Should().HaveCount(7);
        
        repository.UpdateTemplate(template);
    }
    
    [Fact]
    public void TaskFighter_Tasks_Tests()
    {
        // Daily Template with TimeBlocks 
        SamuraiTasksContext samuraiTasksContext = TestHelpers.CreateTestContext();
        
        samuraiTasksContext.AddTask(new TodoTask() { Name = "Test Tasks" });
        samuraiTasksContext.AddTask(new TodoTask() { Name = "Test Tasks 2" });
        samuraiTasksContext.SaveChanges();
       
        // tf add task c:perso Dev' the apps
        // tf add task c:perso Nettoyer le poêle
        // tf add task c:perso Nettoyer le poêle
        
        // Récupérer les recurring worklows de Notion 
        // Créer un Ritual tous les jours, qui dure 30 ", sur le contexte perso, il sera calé sur un time block qui autorise le perso 
        // > tf create job:daily d:30 c:perso
        // creer un job tous les jours, de 7h à 8:30:
        // > tf create job:daily b:7-83 
        // créer un job tous les jours, de 7h à 8:30 (b pour time block)
        // > tf create job:daily b:7-83 
        
        // Créer un event  
        // tf create event @today:1100 
        // tf create event 22/12/2022:1000 "Aller chercher le gateau de Noel" créer un event à 10h, par défaut d'1h 
        // tf create event 22/12/2022 "Taper"  créer un event le 22, toute la journée si pas d'heure 
        // tf create event 22/12/2022:1120-1230 créer un event le 22 de 11h20 à 12h30

        // Lier la tâche d'ID 1 à l'event 12, comme pré-requis (donc la date de début de l'event entrera en compte lors de la priorisation)
        // > tf link 1 12   
        // Lier le job d'ID 2 à l'event 12, comme pré-requis (donc la date de début de l'event entrera en compte lors de la priorisation des tâches générées par le job)
        
        // permettre de créer des batchs de commandes à partir d'un fichier texte via  NVim ou autre
        // PSv3+:
        // # -Raw reads the entire file into a single string
        // & { Invoke-Expression (Get-Content -Raw C:\execute.txt) } | tf 

        // Démarrer une journée: 
        // > tf start  

        // gestion des conflits horaires
        /* 
        TodoContext context = new TodoContext(DateTime.Now, filePath);
        context.StartDay(); 
        // Sync Notion Tasks from API
        List<Task> notionTasks = NotionService.GetCurrentTasks();
        List<Task> workflowTasks = NotionService.GetWorkflowTasks();
        
        // Récupère l'agenda de la journée à partir du Jour courant
        DaySchedule daySchedule = DayScheduleService.GetDaySchedule(DateTime.Now);
        int workLoad = 400; // charge de travail que je peux accomplir en minutes
        // Priorise et ordonne les task en fonction de l'algo - et retourne les X premières tâches qui remplissent la journée 
        TodoList todayWork = new TodoList(daySchedule, workLoad, notionTasks.AddRange(workflowTasks));
        // dans TodoList
        daySchedule.Fill();
         
        context.StartDay(todayWork);
        int taskId = 1; 
        int taskId2 = 2; 
        context.Switch(taskId, taskId2);
        // Lorsqu'on termine une task , modifie son status et arrête le timer
        context.EndTask(taskId);
        context.StartTask(taskId);
        // Mise à jour avec le status 
        context.EndDay(todayWork);
        */
    }
    
    [Fact]
    public void TaskFighter_Events_Tests()
    {
        // Daily Template with TimeBlocks 
        string configDir = TestConfiguration.GetSecretValue("ConfigPath");
        
        TaskSamuraiConfig config = new TaskSamuraiConfig()
        {
           CurrentIndex = 0,
           ConfigPath = configDir + "test.config.json",
           TodosPath =  configDir + "tasks.json", 
           CalendarPath =  configDir + "calendar.json", 
           LoggerPath = configDir + "database.json",
           Context = "test"
        };
        
        SamuraiTasksContext samuraiTasksContext = new SamuraiTasksContext(config);
        samuraiTasksContext.AddEvent(new TodoEvent() { Name = "Test Events" });
        samuraiTasksContext.AddEvent(new TodoEvent() { Name = "Test Event 2" });
        
        samuraiTasksContext.SaveChanges();
       
        // tf add task c:perso Dev' the apps
        // tf add task c:perso Nettoyer le poêle
        // tf add task c:perso Nettoyer le poêle
        
        // Récupérer les recurring worklows de Notion 
        // Créer un Ritual tous les jours, qui dure 30 ", sur le contexte perso, il sera calé sur un time block qui autorise le perso 
        // > tf create job:daily d:30 c:perso
        // creer un job tous les jours, de 7h à 8:30:
        // > tf create job:daily b:7-83 
        // créer un job tous les jours, de 7h à 8:30 (b pour time block)
        // > tf create job:daily b:7-83 
        
        // Créer un event  
        // tf create event @today:1100 
        // tf create event 22/12/2022:1000 "Aller chercher le gateau de Noel" créer un event à 10h, par défaut d'1h 
        // tf create event 22/12/2022 "Taper"  créer un event le 22, toute la journée si pas d'heure 
        // tf create event 22/12/2022:1120-1230 créer un event le 22 de 11h20 à 12h30

        // Lier la tâche d'ID 1 à l'event 12, comme pré-requis (donc la date de début de l'event entrera en compte lors de la priorisation)
        // > tf link 1 12   
        // Lier le job d'ID 2 à l'event 12, comme pré-requis (donc la date de début de l'event entrera en compte lors de la priorisation des tâches générées par le job)
        
        // permettre de créer des batchs de commandes à partir d'un fichier texte via  NVim ou autre
        // PSv3+:
        // # -Raw reads the entire file into a single string
        // & { Invoke-Expression (Get-Content -Raw C:\execute.txt) } | tf 

        // Démarrer une journée: 
        // > tf start  

        // gestion des conflits horaires
        /* 
        TodoContext context = new TodoContext(DateTime.Now, filePath);
        context.StartDay(); 
        // Sync Notion Tasks from API
        List<Task> notionTasks = NotionService.GetCurrentTasks();
        List<Task> workflowTasks = NotionService.GetWorkflowTasks();
        
        // Récupère l'agenda de la journée à partir du Jour courant
        DaySchedule daySchedule = DayScheduleService.GetDaySchedule(DateTime.Now);
        int workLoad = 400; // charge de travail que je peux accomplir en minutes
        // Priorise et ordonne les task en fonction de l'algo - et retourne les X premières tâches qui remplissent la journée 
        TodoList todayWork = new TodoList(daySchedule, workLoad, notionTasks.AddRange(workflowTasks));
        // dans TodoList
        daySchedule.Fill();
         
        context.StartDay(todayWork);
        int taskId = 1; 
        int taskId2 = 2; 
        context.Switch(taskId, taskId2);
        // Lorsqu'on termine une task , modifie son status et arrête le timer
        context.EndTask(taskId);
        context.StartTask(taskId);
        // Mise à jour avec le status 
        context.EndDay(todayWork);
        */
    }
    
    [Fact]
    public void TaskFighter_Rituals_Tests()
    {
        string configDir = TestConfiguration.GetSecretValue("ConfigPath");
        
        TaskSamuraiConfig config = new TaskSamuraiConfig()
        {
           CurrentIndex = 0,
           ConfigPath = configDir + "test.config.json",
           TodosPath =  configDir + "tasks.json", 
           CalendarPath =  configDir + "calendar.json", 
           RitualsPath =  configDir + "rituals.json", 
           LoggerPath = configDir + "database.json",
           Context = "test"
        };
        
        SamuraiTasksContext samuraiTasksContext = new SamuraiTasksContext(config);
        samuraiTasksContext.AddRitual(new Ritual() { Name = "Test Ritual" });
        samuraiTasksContext.AddRitual(new Ritual() { Name = "Test Ritual 2" });
         
        samuraiTasksContext.SaveChanges();
    }
    
    private static DaySchedule CreateWorkDaySchedule()
    {
        DaySchedule workDay = new DaySchedule()
        {
            TimeBlocks = new List<TimeBlock>()
            {
                new TimeBlock() {Interval = new TimeInterval() {StartTime = new Time(7, 0), EndTime = new Time(8, 30)}},
                new TimeBlock() {Interval = new TimeInterval() {StartTime = new Time(8, 30), EndTime = new Time(10, 00)}},
                new TimeBlock() {Interval = new TimeInterval() {StartTime = new Time(10, 00), EndTime = new Time(10, 30)}},
                new TimeBlock() {Interval = new TimeInterval() {StartTime = new Time(10, 30), EndTime = new Time(12, 30)}},
                new TimeBlock() {Interval = new TimeInterval() {StartTime = new Time(14, 00), EndTime = new Time(16, 00)}},
                new TimeBlock() {Interval = new TimeInterval() {StartTime = new Time(16, 00), EndTime = new Time(17, 30)}},
                new TimeBlock() {Interval = new TimeInterval() {StartTime = new Time(18, 00), EndTime = new Time(20, 00)}},
            }
        };
        return workDay;
    }
}