// <copyright file="ParticleEmitterTokenizer.cs" company="Urs Müller">
// Copyright (c) Urs Müller. All rights reserved.
// </copyright>

namespace AdvancedParticleSystemMG38_1.ParticleSystem.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework;
    using SpecialEffectsExamplesLibrary;

    public enum LocalBlendState
    {
        One,
        Zero,
        SourceColor,
        InverseSourceColor,
        SourceAlpha,
        InverseSourceAlpha,
        DestinationColor,
        InverseDestinationColor,
        DestinationAlpha,
        InverseDestinationAlpha,
        BlendFactor,
        InverseBlendFactor,
        SourceAlphaSaturation,
    }

    public enum TokenizerState
    {
        InWhiteSpace = 1,
        InText,
        InQuote,
        InComment,
    }

    public struct BLENDINGMODE
    {
        public string Name;
        public LocalBlendState Mode;
    }

    public class ParticleEmitterTokenizer
    {
        private const int NUMBLENDINGMODES = 13;

        private static BLENDINGMODE[] blendingModes =
        {
            new BLENDINGMODE { Name = "D3DBLEND_ONE", Mode = LocalBlendState.One },
            new BLENDINGMODE { Name = "D3DBLEND_ZERO", Mode = LocalBlendState.Zero },
            new BLENDINGMODE { Name = "D3DBLEND_SRCCOLOR", Mode = LocalBlendState.SourceColor },
            new BLENDINGMODE { Name = "D3DBLEND_INVSRCCOLOR", Mode = LocalBlendState.InverseSourceColor },
            new BLENDINGMODE { Name = "D3DBLEND_SRCALPHA", Mode = LocalBlendState.SourceAlpha },
            new BLENDINGMODE { Name = "D3DBLEND_INVSRCALPHA", Mode = LocalBlendState.InverseSourceAlpha },
            new BLENDINGMODE { Name = "D3DBLEND_DESTCOLOR", Mode = LocalBlendState.DestinationColor },
            new BLENDINGMODE { Name = "D3DBLEND_INVDESTCOLOR", Mode = LocalBlendState.InverseDestinationColor },
            new BLENDINGMODE { Name = "D3DBLEND_DESTALPHA", Mode = LocalBlendState.DestinationAlpha },
            new BLENDINGMODE { Name = "D3DBLEND_INVDESTALPHA", Mode = LocalBlendState.InverseDestinationAlpha },
            new BLENDINGMODE { Name = "D3DBLEND_BLENDFACTOR", Mode = LocalBlendState.BlendFactor },
            new BLENDINGMODE { Name = "D3DBLEND_INVBLENDFACTOR", Mode = LocalBlendState.InverseBlendFactor },
            new BLENDINGMODE { Name = "D3DBLEND_SRCALPHASAT", Mode = LocalBlendState.SourceAlphaSaturation },
        };

        public List<ParticleEmitterToken> TokenVector = new ();

        public static MinMax<float> ProcessNumber(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            MinMax<float>? number = new ();

            // the first token is either the random keyword, or it's an actual number.
            switch (tokenIter.Current.Type)
            {
                case TokenType.KeywordRandom:
                    // parse random number into minmax
                    ParticleEmitterTokenizer.NextToken(ref tokenIter);

                    if (tokenIter.Current.Type != TokenType.OpenParen)
                    {
                        throw new Exception("Expecting opening paren after Random keyword.");
                    }

                    ParticleEmitterTokenizer.NextToken(ref tokenIter);

                    if (tokenIter.Current.Type != TokenType.RealNumber)
                    {
                        throw new Exception("Expecting first number within Random(...).");
                    }

                    number.Min = float.Parse(tokenIter.Current.StringValue, CultureInfo.InvariantCulture.NumberFormat);

                    ParticleEmitterTokenizer.NextToken(ref tokenIter);

                    if (tokenIter.Current.Type != TokenType.Comma)
                    {
                        throw new Exception("Expecting comma within Random(...).");
                    }

                    ParticleEmitterTokenizer.NextToken(ref tokenIter);

                    if (tokenIter.Current.Type != TokenType.RealNumber)
                    {
                        throw new Exception("Expecting second number within Random(...).");
                    }

                    number.Max = float.Parse(tokenIter.Current.StringValue, CultureInfo.InvariantCulture.NumberFormat);

                    ParticleEmitterTokenizer.NextToken(ref tokenIter);

                    if (tokenIter.Current.Type != TokenType.CloseParen)
                    {
                        throw new Exception("Missing close paren on Random(...).");
                    }

                    ParticleEmitterTokenizer.NextToken(ref tokenIter);

                    break;

                case TokenType.RealNumber:
                    // min and max both equal realnumber
                    if (tokenIter.Current.Type != TokenType.RealNumber)
                    {
                        throw new Exception("Expecting number.");
                    }

                    number.Max = number.Min = float.Parse(tokenIter.Current.StringValue, CultureInfo.InvariantCulture.NumberFormat);

                    ParticleEmitterTokenizer.NextToken(ref tokenIter);

                    break;

                default:
                    throw new Exception("Expecting either Random(...) or a number value.");
            }

            return number;
        }

        public static MinMax<Vector3> ProcessVector(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            // this token needs to be a XYZ keyword.
            if (tokenIter.Current.Type != TokenType.KeywordXYZ)
            {
                throw new Exception("Expecting XYZ(...)!");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            if (tokenIter.Current.Type != TokenType.OpenParen)
            {
                throw new Exception("Expecting ( after XYZ!");
            }

            MinMax<float>? x;
            MinMax<float>? y;
            MinMax<float>? z;

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            x = ProcessNumber(ref tokenIter);

            if (tokenIter.Current.Type != TokenType.Comma)
            {
                throw new Exception("Vector components must be seperated by a comma.");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            y = ProcessNumber(ref tokenIter);

            if (tokenIter.Current.Type != TokenType.Comma)
            {
                throw new Exception("Vector components must be seperated by a comma.");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            z = ProcessNumber(ref tokenIter);

            if (tokenIter.Current.Type != TokenType.CloseParen)
            {
                throw new Exception("Expecting ) to close vector.");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            MinMax<Vector3>? v = new ()
            {
                Min = new Vector3(x.Min, y.Min, z.Min),
                Max = new Vector3(x.Max, y.Max, z.Max),
            };

            return v;
        }

        public static MinMax<Color> ProcessColor(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            MinMax<Color>? c = new ();

            // this token needs to be a RGBA keyword.
            if (tokenIter.Current.Type != TokenType.KeywordColor)
            {
                throw new Exception("Expecting RGBA(...)!");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            if (tokenIter.Current.Type != TokenType.OpenParen)
            {
                throw new Exception("Expecting ( after RGBA!");
            }

            MinMax<float>? r = new ();
            MinMax<float>? g = new ();
            MinMax<float>? b = new ();
            MinMax<float>? a = new ();

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            r = ProcessNumber(ref tokenIter);

            if (tokenIter.Current.Type != TokenType.Comma)
            {
                throw new Exception("Color components must be seperated by a comma.");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            g = ProcessNumber(ref tokenIter);
            if (tokenIter.Current.Type != TokenType.Comma)
            {
                throw new Exception("Color components must be seperated by a comma.");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            b = ProcessNumber(ref tokenIter);
            if (tokenIter.Current.Type != TokenType.Comma)
            {
                throw new Exception("Color components must be seperated by a comma.");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            a = ProcessNumber(ref tokenIter);
            if (tokenIter.Current.Type != TokenType.CloseParen)
            {
                throw new Exception("Expecting ) to close vector.");
            }

            ParticleEmitterTokenizer.NextToken(ref tokenIter);

            c.Min = new Color(r.Min, g.Min, b.Min, a.Min);
            c.Max = new Color(r.Max, g.Max, b.Max, a.Max);

            return c;
        }

        public static void NextToken(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            var ended = tokenIter.MoveNext();

            if (!ended)
            {
                throw new Exception("Unexpected end - of - file.");
            }
        }

        public static bool ProcessTime(ref MinMax<float> timeRange, ref bool isFade, float initialTime, float finalTime, ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            // determine if this time specification is a fade directive
            if (tokenIter.Current.Type == TokenType.KeywordFade)
            {
                // it is... the next token must be "so"
                ParticleEmitterTokenizer.NextToken(ref tokenIter);

                if (tokenIter.Current.Type != TokenType.KeywordSo)
                {
                    throw new Exception("Expecting \"so\" after \"fade\".");
                }

                // flip the fade flag on
                isFade = true;

                // move to next token (so that we land on "at" for the code below)
                ParticleEmitterTokenizer.NextToken(ref tokenIter);
            }
            else
            {
                isFade = false; // just to be safe
            }

            switch (tokenIter.Current.Type)
            {
                case TokenType.KeywordAt:
                    {
                        // easy, just grab the time
                        ParticleEmitterTokenizer.NextToken(ref tokenIter);

                        timeRange = ProcessNumber(ref tokenIter);
                    }

                    break;

                case TokenType.KeywordInitial:
                    {
                        // use initial time that was passed in.
                        if (isFade)
                        {
                            throw new Exception("Cannot use \"fade so\" modifier on \"initial\" times.");
                        }

                        timeRange.Min = initialTime;
                        timeRange.Max = initialTime;

                        ParticleEmitterTokenizer.NextToken(ref tokenIter);
                    }

                    break;

                case TokenType.KeywordFinal:
                    {
                        // use final time that was passed in.
                        timeRange.Min = finalTime;
                        timeRange.Max = finalTime;

                        ParticleEmitterTokenizer.NextToken(ref tokenIter);
                    }

                    break;

                default:
                    throw new Exception("Expected time specifier: \"[fade so] at\", \"initial\", or \"[fade so] final\"");
            }

            return true;
        }

        public static LocalBlendState ProcessAlphaBlendMode(ref List<ParticleEmitterToken>.Enumerator tokenIter)
        {
            if (tokenIter.Current.Type != TokenType.AlphaBlendMode)
            {
                throw new Exception("Expecting alpha blend mode (D3DBLEND_ZERO, D3DBLEND_ONE, etc.).");
            }

            LocalBlendState alphaBlendMode;

            for (int q = 0; q < NUMBLENDINGMODES; q++)
            {
                // careful here... must match on both string and size
                // (i.e., to differentiate between D3DBLEND_SRCALPHA and D3DBLEND_SRCALPHASAT).
                if (tokenIter.Current.StringValue.Contains(blendingModes[q].Name) &&
                    tokenIter.Current.StringValue.Length == blendingModes[q].Name.Length)
                {
                    alphaBlendMode = blendingModes[q].Mode;

                    ParticleEmitterTokenizer.NextToken(ref tokenIter);

                    return alphaBlendMode;
                }
            }

            throw new Exception("Invalid alpha blending mode!");
        }

        public void Tokenize(string str)
        {
            var cs = TokenizerState.InWhiteSpace;
            string? remainingText = str;
            var curCharacter = 0;
            ParticleEmitterToken? token = new ();

            while (curCharacter < remainingText.Length)
            {
                switch (cs)
                {
                    case TokenizerState.InWhiteSpace:
                        {
                            // if this letter is not whitespace,
                            if (!char.IsWhiteSpace(str[curCharacter]) && !char.IsControl(str[curCharacter]))
                            {
                                // add it to the running buffer
                                token.StringValue += str[curCharacter];

                                // switch to appropriate case
                                if (str[curCharacter] == '/' && str[curCharacter + 1] == '/')
                                {
                                    cs = TokenizerState.InComment;
                                }
                                else
                                {
                                    cs = (str[curCharacter] == '\"') ? TokenizerState.InQuote : TokenizerState.InText;
                                }
                            }
                        }

                        break;

                    case TokenizerState.InText:
                        {
                            // if this letter is whitespace
                            if (char.IsWhiteSpace(str[curCharacter]))
                            {
                                // add the completed token to the vector
                                AddToken(token);

                                // Reset the token
                                token = new ParticleEmitterToken();

                                // switch to whitespace case
                                cs = TokenizerState.InWhiteSpace;
                            }
                            else
                            {
                                // if this letter is a token terminator
                                if (str[curCharacter] == '(' || str[curCharacter] == ')' || str[curCharacter] == ',' || str[curCharacter] == '\"' || str[curCharacter] == '{' || str[curCharacter] == '}' || str[curCharacter] == '/')
                                {
                                    if (str[curCharacter] == '/' && str[curCharacter + 1] == '/')
                                    {
                                        cs = TokenizerState.InComment;
                                    }
                                    else
                                    {
                                        // add the completed token to the vector
                                        AddToken(token);

                                        // Reset the token
                                        token = new ParticleEmitterToken();

                                        // if it was a quote, transition to InQuote state
                                        if (str[curCharacter] == '\"')
                                        {
                                            cs = TokenizerState.InQuote;
                                        }

                                        // otherwise, process this one char as a token
                                        else
                                        {
                                            token.StringValue = str[curCharacter].ToString();
                                            AddToken(token);
                                            token = new ParticleEmitterToken
                                            {
                                                StringValue = string.Empty,
                                            };
                                        }
                                    }
                                }
                                else
                                {
                                    // add this letter to the work in progress token
                                    token.StringValue += str[curCharacter];
                                }
                            }
                        }

                        break;

                    case TokenizerState.InComment:
                        {
                            // C++ style comments - skip everything until end of line
                            if (str[curCharacter] == '\n')
                            {
                                token.StringValue = string.Empty;
                                cs = TokenizerState.InWhiteSpace;
                            }
                        }

                        break;

                    case TokenizerState.InQuote:
                        {
                            // unconditionally add this letter to the token until we hit a close quote
                            token.StringValue += str[curCharacter];
                            if (str[curCharacter] == '\"')
                            {
                                AddToken(token);

                                // Reset the token
                                token = new ParticleEmitterToken();

                                // go back to whitespace
                                cs = TokenizerState.InWhiteSpace;
                            }
                        }

                        break;
                }

                curCharacter++;
            }

            AddToken(token);
        }

        private void AddToken(ParticleEmitterToken token)
        {
            if (token.StringValue != null)
            {
                if (token.StringValue.Length > 0)
                {
                    DetermineTokenType(ref token);
                    TokenVector.Add(token);
                }
            }
        }

        private void DetermineTokenType(ref ParticleEmitterToken token)
        {
            token.Type = TokenType.Unknown;

            // these things are easy to see...
            if (char.IsDigit(token.StringValue[0]) || token.StringValue[0] == '-')
            {
                token.Type = TokenType.RealNumber;
                return;
            }

            if (token.StringValue[0] == '=')
            {
                token.Type = TokenType.Equals;
                return;
            }

            if (token.StringValue[0] == ',')
            {
                token.Type = TokenType.Comma;
                return;
            }

            if (token.StringValue[0] == '(')
            {
                token.Type = TokenType.OpenParen;
                return;
            }

            if (token.StringValue[0] == ')')
            {
                token.Type = TokenType.CloseParen;
                return;
            }

            if (token.StringValue[0] == '{')
            {
                token.Type = TokenType.OpenBrace;
                return;
            }

            if (token.StringValue[0] == '}')
            {
                token.Type = TokenType.CloseBrace;
                return;
            }

            if (token.StringValue[0] == '\"')
            {
                token.Type = TokenType.Quote;
                return;
            }

            // if we got here, it's not a quote... so convert it to uppercase.
            token.StringValue = token.StringValue.ToUpper();

            // keywords are easy to figure out, just look for the text...
            if (token.StringValue.Contains("PARTICLESYSTEM"))
            {
                token.Type = TokenType.KeywordParticleSystem;
                return;
            }

            if (token.StringValue.Contains("EVENTSEQUENCE"))
            {
                token.Type = TokenType.KeywordEventSequence;
                return;
            }

            if (token.StringValue.Contains("RANDOM"))
            {
                token.Type = TokenType.KeywordRandom;
                return;
            }

            if (token.StringValue.Contains("XYZ"))
            {
                token.Type = TokenType.KeywordXYZ;
                return;
            }

            if (token.StringValue.Contains("RGBA"))
            {
                token.Type = TokenType.KeywordColor;
                return;
            }

            if (token.StringValue.Contains("FADE"))
            {
                token.Type = TokenType.KeywordFade;
                return;
            }

            if (token.StringValue.Contains("INITIAL"))
            {
                token.Type = TokenType.KeywordInitial;
                return;
            }

            if (token.StringValue.Contains("FINAL"))
            {
                token.Type = TokenType.KeywordFinal;
                return;
            }

            if (token.StringValue.Contains("TEXTURE"))
            {
                token.Type = TokenType.KeywordTexture;
                return;
            }

            // these two keywords are embedded in other words, so we've got to be careful.
            if (token.StringValue.Contains("SO") && token.StringValue.Length == 2)
            {
                token.Type = TokenType.KeywordSo;
                return;
            }

            if (token.StringValue.Contains("AT") && token.StringValue.Length == 2)
            {
                token.Type = TokenType.KeywordAt;
                return;
            }

            // now it gets harder...
            if (token.StringValue.Contains("D3DBLEND_"))
            {
                token.Type = TokenType.AlphaBlendMode;
                return;
            }

            if (token.StringValue.Contains("SOURCEBLENDMODE") ||
                token.StringValue.Contains("DESTBLENDMODE"))
            {
                token.Type = TokenType.SeqAlphaBlendModeProp;
                return;
            }

            if (token.StringValue.Contains("LIFETIME") ||
                token.StringValue.Contains("EMITRATE") ||
                token.StringValue.Contains("NUMPARTICLES") ||
                token.StringValue.Contains("LOOPS"))
            {
                token.Type = TokenType.SeqNumericProp;
                return;
            }

            if (token.StringValue.Contains("SPAWNDIR") ||
                token.StringValue.Contains("GRAVITY") ||
                token.StringValue.Contains("EMITRADIUS"))
            {
                token.Type = TokenType.SeqVectorProp;
                return;
            }

            if (token.StringValue.Contains("POSITION"))
            {
                token.Type = TokenType.SysVectorProp;
                return;
            }

            if (token.StringValue.Contains("SIZE") ||
                token.StringValue.Contains("EVENTTIMER"))
            {
                token.Type = TokenType.ParticleNumericProp;
                return;
            }

            if (token.StringValue.Contains("VELOCITY"))
            {
                token.Type = TokenType.ParticleVectorProp;
                return;
            }

            if (token.StringValue.Contains("COLOR"))
            {
                token.Type = TokenType.ParticleColorProp;
                return;
            }
        }
    }
}
