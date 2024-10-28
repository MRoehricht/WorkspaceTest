using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<MovieRepository>();

var app = builder.Build();

app.MapGet("/movies", (MovieRepository repo) => repo.GetAll());
app.MapGet("/movies/{id}", (int id, MovieRepository repo) => repo.GetById(id));
app.MapPost("/movies", (Movie movie, MovieRepository repo) => repo.Add(movie));
app.MapPut("/movies/{id}", (int id, Movie movie, MovieRepository repo) => repo.Update(id, movie));
app.MapDelete("/movies/{id}", (int id, MovieRepository repo) => repo.Delete(id));

app.Run();

public record Movie(int Id, string Title, string Director, int ReleaseYear);

public class MovieRepository
{
    private readonly List<Movie> _movies = new();

    public IEnumerable<Movie> GetAll() => _movies;

    public Movie GetById(int id) => _movies.Find(movie => movie.Id == id);

    public void Add(Movie movie) => _movies.Add(movie);

    public void Update(int id, Movie updatedMovie)
    {
        var index = _movies.FindIndex(movie => movie.Id == id);
        if (index != -1)
        {
            _movies[index] = updatedMovie;
        }
    }

    public void Delete(int id) => _movies.RemoveAll(movie => movie.Id == id);
}
