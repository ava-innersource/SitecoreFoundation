using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SF.Foundation.Container;

namespace SF.Feature.FoundationComponents.Container
{
    public class FoundationContainerClassProcessor
    {
        private const string FoundationTemplate = "{E326352E-369E-455F-AC13-C9986F8683FE}";

        public void Process(ContainerClassPipelineArgs pipelineArgs)
        {
            //Don't add show/hide classes in page editor mode
            if (Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                return;
            }

            //only apply if descendant from FoundationRenderingParameters
            if (pipelineArgs.RenderingParametersTemplate.ID.ToGuid().ToString("B").ToUpper() == FoundationTemplate ||
                pipelineArgs.RenderingParametersTemplate.DescendsFrom(new Sitecore.Data.ID(FoundationTemplate)))
            {
                
                #region show parameters
                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForSmallOnly"))
                {
                    pipelineArgs.CssClasses.Add("show-for-small-only");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForMediumUp"))
                {
                    pipelineArgs.CssClasses.Add("show-for-medium-up");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForMediumOnly"))
                {
                    pipelineArgs.CssClasses.Add("show-for-medium-only");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForLargeUp"))
                {
                    pipelineArgs.CssClasses.Add("show-for-large-up");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForLargeOnly"))
                {
                    pipelineArgs.CssClasses.Add("show-for-large-only");
                }


                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForXLargeUp"))
                {
                    pipelineArgs.CssClasses.Add("show-for-xlarge-up");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForXLargeOnly"))
                {
                    pipelineArgs.CssClasses.Add("show-for-xlarge-only");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForXXLargeUp"))
                {
                    pipelineArgs.CssClasses.Add("show-for-xxlarge-up");
                }
                #endregion

                #region hide parameters
                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForSmallOnly"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-small-only");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForMediumUp"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-medium-up");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForMediumOnly"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-medium-only");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForLargeUp"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-large-up");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForLargeOnly"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-large-only");
                }


                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForXLargeUp"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-xlarge-up");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForXLargeOnly"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-xlarge-only");
                }

                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForXXLargeUp"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-xxlarge-up");
                }
                #endregion

                #region device parameters
                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForLandscape"))
                {
                    pipelineArgs.CssClasses.Add("show-for-landscape");
                }
                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForPortrait"))
                {
                    pipelineArgs.CssClasses.Add("show-for-portrait");
                }
                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForTouch"))
                {
                    pipelineArgs.CssClasses.Add("show-for-touch");
                }
                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForTouch"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-touch");
                }
                if (pipelineArgs.GetCheckboxRenderingParameterValue("ShowForPrint"))
                {
                    pipelineArgs.CssClasses.Add("show-for-print");
                }
                if (pipelineArgs.GetCheckboxRenderingParameterValue("HideForPrint"))
                {
                    pipelineArgs.CssClasses.Add("hide-for-print");
                }
                #endregion
            }
        }
    }
}
