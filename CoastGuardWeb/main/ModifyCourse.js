var CourseID;
var ModeAdd = false;
$(document).ready(function() {
//debugger;
CourseID = sessionID = $.getURLParam('CourseId');
if (typeof(CourseID) == 'undefined'||CourseID==null) ModeAdd = true;
setupForm();
});

function setupForm() {
var Courses=top.getCourses();
//debugger;
if (ModeAdd) return;
var CourseInfo = ArrayFind(Courses, CourseID);
$('input#Name').val(CourseInfo.Name);

}

function Cancel_onclick() {
    top.PopPage('FindCourses.htm');
}
function Save_onclick() {
//debugger;
var Course;
Course=top.BlankCourse;
if (typeof(CourseID) != 'undefined' && CourseID != null) Course.Id = CourseID;
Course.Name = $('input#Name').val();
top.modifyCourse(Course);
top.getCourses(true);
top.PopPage('FindCourses.htm');
}
