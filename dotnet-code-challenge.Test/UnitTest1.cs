using System;
using Xunit;
using dotnet_code_challenge.DataService;

namespace dotnet_code_challenge.Test
{
    public class UnitTest1
    {
        [Fact]
        public void ReadingInvalidFolderForFiles()
        {
            //arrange
   
            //act
            var actual = ReadFileName.ReadFileNames("", "");

            //assert
            Assert.Null(actual);
        }

        [Theory]
        [InlineData("C:\\", "*.abc", 0)]
        [InlineData("C:\\FeedData\\", "xml", 1)]
        public void ReadingValidFolderForFiles(string folder, string types, int expected)
        {
            //arrange

            //act
            var actual = ReadFileName.ReadFileNames(folder, types).Count;

            //assert
            Assert.Equal(actual, expected);
        }

        [Fact]
        public void RaceService_getHorseTest()
        {
            //arrange

            //act

            //assert
        }
    }
}
