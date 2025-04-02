using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace IMDB_Browser.Models;

[PrimaryKey("TitleId", "GenreId")]
[Table("Title_Genres")]
public partial class TitleGenre
{
    [Key]
    [Column("titleID")]
    [StringLength(10)]
    [Unicode(false)]
    public string TitleId { get; set; } = null!;

    [Key]
    [Column("genreID")]
    public int GenreId { get; set; }

    [ForeignKey("TitleId")]
    [InverseProperty("TitleGenres")]
    public virtual Title Title { get; set; } = null!;
}
